using API.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using API.Models.Interfaces;

namespace API.Models
{
    public class Resource : IResource
    {
        [BsonElement("_id")]
        [JsonProperty("_id")]
        [BsonId]
        public ObjectId Id { get; set; }
        [BsonElement("name")]
        public string Name { get; set; }
        [BsonElement("locations")]
        public string[]? Location { get; set; }
        [BsonElement("type")]
        public string[] Type { get; set; }
        [BsonElement("masterable")]
        public bool Mastery { get; set; }
        [BsonIgnore]
        public string strId { get { return Id.ToString(); } }
        [BsonIgnore]
        public int Owned { get; set; } = 0;
        [BsonIgnore]
        private static readonly string[] Ex = { "62d8682daeef469267d8084f", "62d8682daeef469267d80850", "62d8682daeef469267d80851", "62d8682daeef469267d8080b" };

        public Resource() { }
        public Resource(ObjectId id, string name, string[] type, string[] location = null, bool mastery = false)
        {
            Id = id;
            Name = name;
            Location = location;
            Type = type;
            Mastery = mastery;
        }

        public static bool IsResource(string[] type)
        {
            if (type.Intersect(Ex).Any()) return false;
            return true;
        }
        public static bool IsResource(Item item)
        {
            if (item.NeededResources != null) return false;
            if (item.Type.Intersect(Ex).Any()) return false;
            return true;
        }
    }
}
