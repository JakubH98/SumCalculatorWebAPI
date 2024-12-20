using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using SumCalculatorWebAPI.Domain;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace SumCalculatorWebAPI
{
    public class MongoRepository<T> : IRepository<T> where T : IEntity
    {
        private readonly IMongoCollection<T> _collection;
        private readonly IMongoDatabase _database;

        public MongoRepository(IOptions<Database> databaseConfig)
        {
            var client = new MongoClient(databaseConfig.Value.ConnectionString);
            _database = client.GetDatabase(databaseConfig.Value.DatabaseName);
            _collection = _database.GetCollection<T>(typeof(T).Name);
        }

        public async Task<int> GetNextIdAsync()
        {
            var counterCollection = _database.GetCollection<BsonDocument>("Counters");
            var filter = Builders<BsonDocument>.Filter.Eq("_id", typeof(T).Name);
            var update = Builders<BsonDocument>.Update.Inc("SequenceValue", 1);

            var options = new FindOneAndUpdateOptions<BsonDocument>
            {
                IsUpsert = true,
                ReturnDocument = ReturnDocument.After
            };

            var result = await counterCollection.FindOneAndUpdateAsync(filter, update, options);
            return result["SequenceValue"].AsInt32;
        }

        public async Task Add(T item)
        {
            if (int.TryParse(item.ID, out _) == false)
            {
                item.ID = (await GetNextIdAsync()).ToString();
            }
            await _collection.InsertOneAsync(item);
        }

        public async Task<T> Get(int id)
        {
            var filter = Builders<T>.Filter.Eq("ID", id.ToString());
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task Update(T item)
        {
            var filter = Builders<T>.Filter.Eq("ID", item.ID);
            await _collection.ReplaceOneAsync(filter, item);
        }

        public async Task Delete(int id)
        {
            var filter = Builders<T>.Filter.Eq("ID", id.ToString());
            await _collection.DeleteOneAsync(filter);
        }

        public async Task<List<T>> GetAll()
        {
            return await _collection.Find(record => true).ToListAsync();
        }
        public async Task<bool> Exists(Expression<Func<T, bool>> predicate)
        {
            return await _collection.Find(predicate).AnyAsync();
        }
        public async Task<T?> GetFirstOrDefault(Expression<Func<T, bool>> predicate)
        {
            return await _collection.Find(predicate).FirstOrDefaultAsync();
        }
    }
}

