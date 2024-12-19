using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace SumCalculatorWebAPI.Domain
{
    public class Method : IEntity
    {
        
        public string? _id { get; set; }
        [JsonPropertyName("id")]
        public string? ID { get; set; }
        [JsonPropertyName("title")]
        public string? Title { get; set; }
        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("artifactList")]
        public List<Item>? ArtifactList { get; set; }
        [JsonPropertyName("actorList")]
        public List<Item>? ActorList { get; set; }
        [JsonPropertyName("todoList")]
        public List<ToDo>? TodoList { get; set; }
    }
    public class Item
    {
        [JsonPropertyName("title")]
        public string? Title { get; set; }
        [JsonPropertyName("description")]
        public string? Description { get; set; }  
    }
    public class ToDo
    {
        [JsonPropertyName("title")]
        public string? Title { get; set; }
        [JsonPropertyName("description")]
        public string? Description { get; set; }
        [JsonPropertyName("isDone")]
        public bool? IsDone { get; set; } = false;
    }
}
