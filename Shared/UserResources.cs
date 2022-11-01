using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace Shared
{
    /// <summary>
    /// Projection class of user stored resources.
    /// </summary>
    public sealed class UserResources
    {
        [BsonIgnore]
        private int _credits = 0;
        /// <summary>
        /// Record in DB id.
        /// </summary>
        [BsonId]
        [BsonElement("_id")]
        public ObjectId? Id { get; }
        /// <summary>
        /// Owner of resources id.
        /// </summary>
        [BsonElement("user")]
        public ObjectId User { get; }
        /// <summary>
        /// User credits number.
        /// </summary>
        [BsonElement("credits")]
        public int Credits
        {
            get => _credits;
            set => _credits = (value > 0) ? value : _credits;
        }
        /// <summary>
        /// Users resource list.
        /// </summary>
        [BsonElement("resources")]
        public Dictionary<string, int> Resources { get; }
        /// <summary>
        /// Users item list.
        /// </summary>
        [BsonElement("items")]
        public Dictionary<string, int> Items { get; }
        [BsonConstructor]
        [JsonConstructor]
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
