using MongoDB.Bson.Serialization.Attributes;
namespace SumCalculatorWebAPI.Domain
{
    public class User : IEntity
    {
        [BsonId]
        public string? ID { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
    }
}
