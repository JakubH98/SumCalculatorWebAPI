using MongoDB.Bson;
using MongoDB.Driver;
using SumCalculatorWebAPI.Domain;
using System.Diagnostics;


namespace SumCalculatorWebAPI
{
    
    public class Program
    {
        public static void Main(string[] args)
        {

            // Read MongoDB settings from configuration
            //var connectionString = builder.Configuration.GetSection("DatabaseSettings:ConnectionString").Value;
            //var databaseName = builder.Configuration.GetSection("DatabaseSettings:DatabaseName").Value;

            // Initialize the MongoDB Singleton
            //MongoDBSingleton.Initialize(connectionString, databaseName);

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.Configure<Database>(
               builder.Configuration.GetSection("DatabaseSettings"));

             builder.Services.AddSingleton<IRepository<User>, MongoRepository<User>>();

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
           // app.UseAuthorization();

            app.MapPost("/api/users", async (IRepository<User> repository, User user) =>
            {
                Debug.WriteLine("test");
                await repository.Add(user);
                return Results.Ok($"User {user.Username} created successfully!");
            });

            app.MapGet("/api/users/{id}", async (IRepository<User> repository, int id) =>
            {
                var user = await repository.Get(id);
                if (user is null)
                {
                    return Results.NotFound("User not found.");
                }

                return Results.Ok(user);
            });




            app.Run();
        }

    }
}
