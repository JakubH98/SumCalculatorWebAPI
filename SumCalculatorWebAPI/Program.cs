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


            var builder = WebApplication.CreateBuilder(args);

            builder.Services.Configure<Database>(
               builder.Configuration.GetSection("DatabaseSettings"));

             builder.Services.AddSingleton<IRepository<User>, MongoRepository<User>>();

            builder.Services.AddAuthorization();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            var app = builder.Build();


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
                await repository.Add(user);
                return Results.Ok($"User {user.Username} created successfully with ID {user.ID}!");
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

            app.MapDelete("api/users/{id}", async (IRepository<User> repository, int id) =>
            {
                var user = await repository.Get(id);
                if (user is null)
                {
                    return Results.NotFound("User not found.");
                }
                await repository.Delete(id);
                return Results.Ok("User deleted.");

            });
            app.MapPut("/api/users/{id}", async (IRepository<User> repository, string id, User updatedUser) =>
            {
                var existingUser = await repository.Get(int.Parse(id));
                if (existingUser is null)
                {
                    return Results.NotFound("User not found.");
                }

                updatedUser.ID = id; // Ensure the updated user keeps the same ID
                await repository.Update(updatedUser);
                return Results.Ok($"User {updatedUser.Username} updated successfully!");
            });
            app.Run();
        }
    }
}
