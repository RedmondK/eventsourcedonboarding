using Domain.Events;
using MongoDAL;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ProjectionFramework
{
    public class CaseProjection : Projection
    {
        public CaseProjection()
        {
            MongoDBRepository projectionRepository = new MongoDBRepository();

            When<CaseInitiated>(e =>
            {
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
