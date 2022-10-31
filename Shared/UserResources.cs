using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Shared
{
    public sealed class UserResources
    {
        [BsonIgnore]
        private int _credits = 0;

        [BsonId]
        [BsonElement("_id")]       
        public ObjectId? Id { get; }
        [BsonElement("user")]
        public ObjectId User { get; }
        [BsonElement("credits")]
        public int Credits 
        {
            get => _credits;
            set => _credits = (value > 0) ? value : _credits; 
        }
        [BsonElement("resources")]
        public Dictionary<string, int> Resources { get; }
        [BsonElement("items")]
        public Dictionary<string, int> Items { get; }
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
