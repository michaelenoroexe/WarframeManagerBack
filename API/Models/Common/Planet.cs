using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace API.Models.Common
{
    /// <summary>
    /// Class projection of planet in DB.
    /// </summary>
    internal sealed class Planet
    {
        [BsonId]
        [BsonElement("_id")]
        public ObjectId Id { get; init; }
        [BsonElement("name")]
        public string Name { get; init; }

        public Planet(ObjectId id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
