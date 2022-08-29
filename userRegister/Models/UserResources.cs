using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace API.Models
{
    public class UserResources
    {
        [BsonElement("_id")]
        [BsonId]
        public ObjectId Id { get; set; }
        [BsonElement("user")]
        public ObjectId User { get; set; }
        [BsonElement("items")]
        //[BsonExtraElements]
        public Dictionary<string, int> Items { get; set; } 
    }
}
