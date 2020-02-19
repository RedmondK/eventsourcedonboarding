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

            try
            {
                var sub = eventStoreConnection.SubscribeToAllFrom(
                    checkpoint,
                    CatchUpSubscriptionSettings.Default,
                    EventAppeared(projection),
                    LiveProcessingStarted(projection),
                    userCredentials: new UserCredentials("admin", "changeit")
                );
            }
            catch(Exception e)
            {
                int x = 0;
            }
        }

        Action<EventStoreCatchUpSubscription> LiveProcessingStarted(IProjection projection)
        {
            return s => Console.WriteLine($"Projection {projection.GetType().Name} has caught up, now processing live");
        }

        Action<EventStoreCatchUpSubscription, ResolvedEvent> EventAppeared(IProjection projection)
        {
            return (s, e) =>
            {
                if(e.Event.EventType != "$statsCollected")
                {
                    Console.WriteLine("Event Appeared: " + e.Event.EventType);
                }

                if (!projection.CanHandle(e.Event.EventType))
                {
                    return;
                }

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
