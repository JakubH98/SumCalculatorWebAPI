using MongoDB.Bson.Serialization.Attributes;

namespace SumCalculatorWebAPI.Domain
{
    public class Project : IEntity
    {
        [BsonId]
        public string? ID { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
    }
}
