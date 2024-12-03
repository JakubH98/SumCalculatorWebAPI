using MongoDB.Bson;
using MongoDB.Driver;
using SumCalculatorWebAPI.Domain;


namespace SumCalculatorWebAPI
{
    
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // Read MongoDB settings from configuration
            var connectionString = builder.Configuration.GetSection("DatabaseSettings:ConnectionString").Value;
            var databaseName = builder.Configuration.GetSection("DatabaseSettings:DatabaseName").Value;

            // Initialize the MongoDB Singleton
            MongoDBSingleton.Initialize(connectionString, databaseName);



            //builder.Services.Configure<Database>(
            //    builder.Configuration.GetSection("DBUserSettings"));

            // builder.Services.AddSingleton<IRepository<User>>();

            builder.Services.AddAuthorization();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                    options.RoutePrefix = "swagger";
                });
            }


            app.UseHttpsRedirection();

            app.UseAuthorization();

                       
            app.Run();
        }

        public int GetSequenceValue(string collectionName, IMongoDatabase _database)
        {

            var counterCollection = _database.GetCollection<BsonDocument>("counters");

            var filter = Builders<BsonDocument>.Filter.Eq("id", collectionName);
            var update = Builders<BsonDocument>.Update.Inc("sequence", 1);

            var options = new FindOneAndUpdateOptions<BsonDocument>
            {
                ReturnDocument = ReturnDocument.After
            };

            var result = counterCollection.FindOneAndUpdate(filter, update, options);

            return result["sequence"].ToInt32();
        }
    }
}
