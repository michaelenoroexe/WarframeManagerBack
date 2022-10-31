using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace API.Models.Common
{
    public class Planet
    {
        [BsonElement("_id")]
        [BsonId]
        public ObjectId Id { get; set; }
        [BsonElement("name")]
        public string Name { get; set; }

    }
}
