using Domain.Enums;
using Domain.Events;
using MongoDAL;
using MongoDB.Bson;
using MongoDB.Driver;
using System;

namespace ProjectionFramework
{
    public class CaseProjection : Projection
    {
        public CaseProjection(MongoDBRepository projectionRepository)
        {
            When<CaseInitiated>(e =>
            {
                if (e.CaseId == null)
                {
                    return;
                }

                projectionRepository.Connect();
                var caseCollection = projectionRepository.GetCollection("Case");

                var document = new BsonDocument
                {
                    { "Id", e.CaseId },
                    { "EntityId", e.EntityId },
                    { "Type", Enum.GetName(typeof(CaseType), e.CaseType) }
                };

                caseCollection.InsertOne(document);
            });
        }
    }
}
