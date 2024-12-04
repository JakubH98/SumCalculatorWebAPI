using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace SumCalculatorWebAPI
{
    public class MongoRepository<T> : IRepository<T>
    {
        private readonly IMongoCollection<T> _collection;

        public MongoRepository(IOptions<Database> databaseConfig)
        {
            var client = new MongoClient(databaseConfig.Value.ConnectionString);
            var database = client.GetDatabase(databaseConfig.Value.DatabaseName);
            _collection = database.GetCollection<T>(databaseConfig.Value.CollectionName);
        }

        public async Task Add(T item)
        {
            await _collection.InsertOneAsync(item);
        }

        public async Task<T> Get(int id)
        {
            var filter = Builders<T>.Filter.Eq("ID", id.ToString());
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }
        public async Task<int> GetSequenceValue(string collectionName)
        {
            var counterCollection = _collection.Database.GetCollection<BsonDocument>("counters");

            var filter = Builders<BsonDocument>.Filter.Eq("id", collectionName);
            var update = Builders<BsonDocument>.Update.Inc("sequence", 1);

            var options = new FindOneAndUpdateOptions<BsonDocument>
            {
                ReturnDocument = ReturnDocument.After,
                IsUpsert = true
            };

            var result = await counterCollection.FindOneAndUpdateAsync(filter, update, options);
            return result["sequence"].ToInt32();
        }
    }
}
