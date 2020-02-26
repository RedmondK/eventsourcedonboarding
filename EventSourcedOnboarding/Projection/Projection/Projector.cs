using EventStore.ClientAPI;
using EventStore.ClientAPI.SystemData;
using EventStoreFramework;
using MongoDAL;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace ProjectionFramework
{
    public class Projector
    {
        readonly IEventStoreConnection eventStoreConnection;
        readonly MongoDBRepository projectionRepository;

        readonly List<IProjection> projections;

        public Projector(IEventStoreConnection connection, List<IProjection> projectionList, MongoDBRepository mongoDBRepository)
        {
            projections = projectionList;
            eventStoreConnection = connection;
            projectionRepository = mongoDBRepository;
        }

        public void Start()
        {
            foreach (var projection in projections)
            {
                StartProjection(projection);
            }
        }

        void StartProjection(IProjection projection)
        {
            var checkpoint = GetPosition(projection.GetType());

            var sub = eventStoreConnection.SubscribeToAllFrom(
                checkpoint,
                new CatchUpSubscriptionSettings(500, 100, true, false),
                EventAppeared(projection),
                LiveProcessingStarted(projection),
                SubDropped(),
                userCredentials: new UserCredentials("admin", "changeit")
            );

            Console.WriteLine("Started Projection: " + projection.GetType().Name);
        }

        Action<EventStoreCatchUpSubscription, SubscriptionDropReason, Exception> SubDropped() {
            return (s,r,e) => { 
                Console.WriteLine("***************************************************************");
                Console.WriteLine("Sub Dropped: " + r.ToString());
                Console.WriteLine("Exception: " + e.Message);
                Console.WriteLine("StackTrace: " + e.StackTrace);
                Console.WriteLine("Inner Exception: " + e.InnerException.Message);
                Console.WriteLine("Inner Exception ST: " + e.InnerException.StackTrace);
                Console.WriteLine("***************************************************************");
            };
        }

        Action<EventStoreCatchUpSubscription> LiveProcessingStarted(IProjection projection)
        {
            return s => Console.WriteLine($"Projection {projection.GetType().Name} has caught up, now processing live");
        }

        Action<EventStoreCatchUpSubscription, ResolvedEvent> EventAppeared(IProjection projection)
        {
            return (s, e) =>
            {
                if (!projection.CanHandle(e.Event.EventType))
                {
                    UpdatePosition(projection.GetType(), e.OriginalPosition.Value);
                    return;
                }

                Console.WriteLine("Processing");
                Console.WriteLine(e.Event.EventStreamId);
                Console.WriteLine(e.Event.EventNumber);
                Console.WriteLine(e.Event.EventType);

                var deserializedEvent = e.Deserialize();
                projection.Handle(e.Event.EventType, deserializedEvent);

                UpdatePosition(projection.GetType(), e.OriginalPosition.Value);
            };
        }

        Position? GetPosition(Type projection)
        {
            projectionRepository.Connect();

            var projectionStateCollection = projectionRepository.GetCollection("ProjectionState");

            var filter = Builders<BsonDocument>.Filter.Eq("Id", projection.Name);
            var count = projectionStateCollection.Find(filter).CountDocuments();

            if (count == 0)
            {
                return new Position(0, 0);
            }

            var stateData = projectionStateCollection.Find(filter).First();
            ProjectionState state = new ProjectionState();
            state.Id = stateData.GetValue("Id").ToString();
            state.CommitPosition = stateData.GetValue("CommitPosition").ToInt64();
            state.PreparePosition = stateData.GetValue("PreparePosition").ToInt64();

            return new Position(state.CommitPosition, state.PreparePosition);
        }

        void UpdatePosition(Type projection, Position position)
        {
            projectionRepository.Connect();

            var projectionStateCollection = projectionRepository.GetCollection("ProjectionState");

            var filter = Builders<BsonDocument>.Filter.Eq("Id", projection.Name);
            var state = projectionStateCollection.Find(filter);
            
            if (state.CountDocuments() == 0)
            {
                var document = new BsonDocument
                {
                    { "Id", projection.Name },
                    { "CommitPosition", position.CommitPosition },
                    { "PreparePosition", position.PreparePosition }
                };
                projectionStateCollection.InsertOne(document);
            }
            else
            {
                var update = Builders<BsonDocument>.Update
                    .Set("CommitPosition", position.CommitPosition)
                    .Set("PreparePosition", position.PreparePosition);

                projectionStateCollection.UpdateOne(filter, update);
            }
        }
    }
}
