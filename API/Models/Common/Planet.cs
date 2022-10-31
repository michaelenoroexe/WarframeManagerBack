using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace API.Models.Common
{
    internal sealed class Planet
    {        
        [BsonId]
        [BsonElement("_id")]
        public ObjectId Id { get; set; }
        [BsonElement("name")]
        public string Name { get; set; }

        public Planet(ObjectId id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
