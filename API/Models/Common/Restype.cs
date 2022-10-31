using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace API.Models.Common
{
    public sealed class Restype
    {   
        [BsonId]
        [BsonElement("_id")]
        public ObjectId Id { get; }
        [BsonElement("name")]
        public string Name { get; }
        [BsonIgnore]
        public string StringID => Id.ToString();

        public Restype(ObjectId id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
