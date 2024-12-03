using MongoDB.Driver;

namespace SumCalculatorWebAPI
{
    public static class MongoDBSingleton
    {
        private static IMongoClient? _client;
        private static IMongoDatabase? _database;

        public static void Initialize(string connectionString, string databaseName)
        {
            _client = new MongoClient(connectionString);
            _database = _client.GetDatabase(databaseName);
        }

        public static IMongoDatabase GetDatabase()
        {
            if (_database == null)
            {
                throw new InvalidOperationException("MongoDB is not initialized. Call Initialize() first.");
            }
            return _database;
        }
    }
}
