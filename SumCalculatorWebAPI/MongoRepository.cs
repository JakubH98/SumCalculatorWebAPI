using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using SumCalculatorWebAPI.Domain;

namespace SumCalculatorWebAPI
{
    public class MongoRepository<T> : IRepository<T> where T : IEntity
    {
        private readonly IMongoCollection<T> _collection;
        private readonly IMongoCollection<BsonDocument> _countersCollection;

        public MongoRepository(IOptions<Database> databaseConfig)
        {
            var client = new MongoClient(databaseConfig.Value.ConnectionString);
            var database = client.GetDatabase(databaseConfig.Value.DatabaseName);
            _collection = database.GetCollection<T>(databaseConfig.Value.CollectionName);
            _countersCollection = database.GetCollection<BsonDocument>("Counters");
        }
        private async Task<string> GetNextIdAsync()
        {
            var filter = Builders<BsonDocument>.Filter.Eq("_id", "userId");
            var update = Builders<BsonDocument>.Update.Inc("value", 1);
            var options = new FindOneAndUpdateOptions<BsonDocument>
            {
                IsUpsert = true, 
                ReturnDocument = ReturnDocument.After
            };

            var result = await _countersCollection.FindOneAndUpdateAsync(filter, update, options);
            return result["value"].ToString();
        }

        public async Task Add(T item)
        {
            if (string.IsNullOrEmpty(item.ID))
            {
                item.ID = await GetNextIdAsync();
            }
            await _collection.InsertOneAsync(item);
        }

        public async Task<T> Get(int id)
        {
            var filter = Builders<T>.Filter.Eq(entity => entity.ID, id.ToString());
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task Delete(int id)
        {
            var filter = Builders<T>.Filter.Eq(entity => entity.ID, id.ToString());
            await _collection.DeleteOneAsync(filter);
        }

        public async Task Update(T item)
        {
            var filter = Builders<T>.Filter.Eq(entity => entity.ID, item.ID);
            await _collection.ReplaceOneAsync(filter, item);
        }
    }
}

