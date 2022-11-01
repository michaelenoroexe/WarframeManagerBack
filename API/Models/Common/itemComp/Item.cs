using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace API.Models.Common.ItemComp
{
    /// <summary>
    /// Full item with craft.
    /// </summary>
    internal sealed class Item : Resource
    {
        [BsonElement("createTime")]
        public int CreationTime { get; init; }
        [BsonElement("credits")]
        public int Credits { get; init; }
        [BsonElement("components")]
        public Dictionary<string, int> NeededResources { get; init; }
        [BsonIgnore]
        private static readonly string[] Ex = { "62d8682daeef469267d8084f", "62d8682daeef469267d80850", "62d8682daeef469267d80851", "62d8682daeef469267d8080b" };

        public Item(ObjectId id, string name, string[] type, int creationTime, int credits, Dictionary<string, int> neededRes,
                    string[]? location = null, bool mastery = false) : base(id, name, type, location, mastery)
        {
            CreationTime = creationTime;
            Credits = credits;
            NeededResources = neededRes;
        }
        /// <summary>
        /// Check type of item.
        /// </summary>
        public bool IsResource()
        {
            if (NeededResources != null) return false;
            if (Type.Intersect(Ex).Any()) return false;
            return true;
        }
    }
}
