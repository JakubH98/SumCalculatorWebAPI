using Microsoft.Extensions.Options;
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
            var filter = Builders<T>.Filter.Eq("ID", id);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }
    }
}
