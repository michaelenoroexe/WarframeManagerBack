using API.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace API.Models
{
    public class Resource
    {
        [BsonElement("_id")]
        [JsonProperty("_id")]
        [BsonId]
        public ObjectId Id { get; set; }
        [BsonElement("name")]
        public string Name { get; set; }
        [BsonElement("location")]
        public string? Location { get; set; }
        [BsonElement("type")]
        public string Type { get; set; }
        [BsonElement("mastery")]
        public bool? Mastery { get; set; }
        
        public Resource() { }
        public Resource(ObjectId id, string name, string type, string location="", bool mastery = false)
        {
            Id = id;
            Name = name;
            Location = location;
            Type = type;
            Mastery = mastery;
        }
    }
}
