using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using SumCalculatorWebAPI.Domain;
using System.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;



namespace SumCalculatorWebAPI
{

    
    public class Program
    {

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            builder.Services.Configure<Database>(builder.Configuration.GetSection("DatabaseSettings"));
            builder.Services.AddSingleton(typeof(IRepository<>), typeof(MongoRepository<>));
            builder.Services.AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "https://localhost:7214",
                    ValidAudience = "https://localhost:7214",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("JakubsTopSecretSuperKeyThatNobodyShouldEverKnowAbout12345"))
                };
            });
            builder.Services.AddAuthorization();
            builder.Services.AddControllers();


            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowReactApp", policy =>
                {
                    policy.WithOrigins("http://localhost:3000") 
                          .AllowAnyHeader()                   
                          .AllowAnyMethod();                  
                });
            });

            var app = builder.Build();

            // Enable CORS 
            app.UseCors("AllowReactApp");
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                    options.RoutePrefix = "swagger";
                });
            }
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.UseHttpsRedirection();
            app.Run();





            // MINIMAL API CALLS
            /*app.MapPost("/api/users", async (IRepository<User> repository, User user) =>
            {
                await repository.Add(user);
                return Results.Ok($"User {user.Username} created successfully! with ID {user.ID}!");
            });

            app.MapGet("/api/users/{id}", async (IRepository<User> repository, int id) =>
            {
                var user = await repository.Get(id);
                if (user is null) return Results.NotFound("User not found.");
                return Results.Ok(user);
            });

            app.MapPut("/api/users", async (IRepository<User> repository, User user) =>
            {
                await repository.Update(user);
                return Results.Ok($"User {user.Username} updated successfully! with ID {user.ID}!");
            });

            app.MapDelete("/api/users/{id}", async (IRepository<User> repository, int id) =>
            {
                await repository.Delete(id);
                return Results.Ok("User deleted successfully.");
            });

           
            app.MapPost("/api/projects", async (IRepository<Project> repository, Project project) =>
            {
                await repository.Add(project);
                return Results.Ok($"Project {project.Title} created successfully!");
            });

            app.MapGet("/api/projects/{id}", async (IRepository<Project> repository, int id) =>
            {
                var project = await repository.Get(id);
                if (project is null) return Results.NotFound("Project not found.");
                return Results.Ok(project);
            });

            app.MapPut("/api/projects", async (IRepository<Project> repository, Project project) =>
            {
                await repository.Update(project);
                return Results.Ok($"Project {project.Title} updated successfully!");
            });

            app.MapDelete("/api/projects/{id}", async (IRepository<Project> repository, int id) =>
            {
                await repository.Delete(id);
                return Results.Ok("Project deleted successfully.");
            });*/
        }
    }
}
