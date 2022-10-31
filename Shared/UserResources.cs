using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Shared
{
    public class UserResources
    {
        [BsonElement("_id")]
        [BsonId]
        public ObjectId? Id { get; set; }
        [BsonElement("user")]
        public ObjectId User { get; set; }
        [BsonElement("credits")]
        public int Credits { get; set; }
        [BsonElement("resources")]
        public Dictionary<string, int> Resources { get; set; }
        [BsonElement("items")]
        public Dictionary<string, int> Items { get; set; }
        /// <summary>
        /// Create user resource.
        /// </summary>
        public UserResources(ObjectId user, ObjectId? id = null,
            int credits = 0, Dictionary<string, int>? resources = null, Dictionary<string, int>? items = null)
        {
            User = user;
            Id = id;
            Credits = credits;
            Resources = resources ?? new Dictionary<string, int>();
            Items = items ?? new Dictionary<string, int>();
        }
    }
}
