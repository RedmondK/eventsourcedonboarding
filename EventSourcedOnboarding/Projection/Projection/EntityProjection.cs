using Domain.Events;
using MongoDAL;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectionFramework
{
    public class EntityProjection : Projection
    {
        public EntityProjection()
        {
            MongoDBRepository projectionRepository = new MongoDBRepository();

            When<EntityCreated>(e =>
            {
                projectionRepository.Connect();
                var entityCollection = projectionRepository.GetCollection("Entity");

                var document = new BsonDocument
                {
                    { "Id", e.EntityId },
                    { "Name", e.EntityName }
                };

                entityCollection.InsertOne(document);
            });

            When<BasicDetailsAdded>(e =>
            {
                projectionRepository.Connect();

                var filter = Builders<BsonDocument>.Filter.Eq("Id", e.EntityId);
                var entityCollection = projectionRepository.GetCollection("Entity");

                var searchResult = entityCollection.Find(filter);

                if (searchResult.CountDocuments() == 0)
                {
                    return;
                }

                var update = Builders<BsonDocument>.Update
                   .Set("DateOfBirth", e.DateOfBirth)
                   .Set("CountryOfResidence", e.CountryOfResidence);

                entityCollection.UpdateOne(filter, update);
            });
        }
    }
}
