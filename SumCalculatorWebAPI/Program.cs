using MongoDB.Bson;
using MongoDB.Driver;

namespace SumCalculatorWebAPI
{
    
    public class Program
    {
        Database _database;
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.Configure<Database>(
                builder.Configuration.GetSection("DBUserSettings"));
            builder.Services.AddSingleton<Database>();

            // Add services to the container.
            builder.Services.AddAuthorization();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();



            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseHttpsRedirection();

            app.UseAuthorization();

                       
            app.Run();
        }

        public int GetSequenceValue(string collectionName)
        {
            IMongoDatabase _database = new IMongoDatabase();

            var counterCollection = _database.GetCollection<BsonDocument>(collectionName);

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
