using Domain.Events;
using MongoDAL;
using MongoDB.Bson;
using MongoDB.Driver;

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
                    { "Type", e.CaseType }
                };

                caseCollection.InsertOne(document);
            });
        }
    }
}
