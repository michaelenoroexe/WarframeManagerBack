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
        private static readonly string ResType = "62d8682daeef469267d807ff";

        public Resource() { }
        public Resource(ObjectId id, string name, string[] type, string[] location = null, bool mastery = false)
        {
            Id = id;
            Name = name;
            Location = location;
            Type = type;
            Mastery = mastery;
        }

        public static bool IsResource(string[] Type)
        {
            if (Type.Contains(ResType)) return true;
            return false;
        }
        public static bool IsResource(Item item)
        {
            if (item.Credits > 0) return false;
            if (!item.Type.Contains(ResType)) return false;
            return true;
        }
    }
}
