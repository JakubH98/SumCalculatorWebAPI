using MongoDB.Bson.Serialization.Attributes;

namespace SumCalculatorWebAPI.Domain
{
    public class Method : IEntity
    {
        [BsonId]
        public string? ID { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public bool? IsDone { get; set; } = false;

        public List<Item>? ArtifactList { get; set; }
        public List<Item>? ActorList { get; set; }
        public List<Item>? TodoList { get; set; }
    }
    public class Item
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
    }
}
