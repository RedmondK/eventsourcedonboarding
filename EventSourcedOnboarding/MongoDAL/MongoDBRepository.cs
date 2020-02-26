using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace MongoDAL
{
    public class MongoDBRepository
    {
        readonly IMongoClient mongoClient;
        private IMongoDatabase database;

        public MongoDBRepository()
        {
            mongoClient = new MongoClient("mongodb://admin:admin@localhost:27017/");
        }

        public MongoDBRepository(string connectionString)
        {
            mongoClient = new MongoClient(connectionString);
        }

        public void Connect()
        {
            database = mongoClient.GetDatabase("eventsourcedonboarding");
        }

        public IMongoCollection<BsonDocument> GetCollection(string collectionName)
        {
            return database.GetCollection<BsonDocument>(collectionName);
        }
    }
}
