using MongoDB.Bson.Serialization.Attributes;
using System.Runtime.ExceptionServices;
namespace SumCalculatorWebAPI.Domain
{
    public class User : IEntity
    {
        [BsonId]
        public string? ID { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public string? CompanyName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

    }
}
