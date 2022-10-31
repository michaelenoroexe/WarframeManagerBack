using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace API.Models.Common.ItemComp
{
    internal class Resource : IResource
    {
        [BsonId]
        [BsonElement("_id")]
        [JsonProperty("_id")]
        public ObjectId Id { get; init; }
        [BsonElement("name")]
        public string Name { get; init; }
        [BsonElement("locations")]
        public string[]? Location { get; init; }
        [BsonElement("type")]
        public string[] Type { get; init; }
        [BsonElement("masterable")]
        public bool Mastery { get; init; }
        [BsonIgnore]
        public int Owned { get; protected set; }
        [BsonIgnore]
        public string StringID => Id.ToString();

        public Resource(ObjectId id, string name, string[] type, string[]? location = null, bool mastery = false)
        {
            Id = id;
            Name = name;
            Location = location;
            Type = type;
            Mastery = mastery;
        }
        /// <summary>
        /// Set user owned number.
        /// </summary>
        /// <param name="number">Positive having number.</param>
        public void SetOwned(int number) => Owned = (number > 0) ? number : Owned;
        public bool Equals(IResource? other) => other is not null && GetHashCode() == other!.GetHashCode();
        public override bool Equals(object? obj)
        {
            if (obj is string itemID) return StringID.Equals(itemID, StringComparison.OrdinalIgnoreCase);
            return obj is IResource resource && Equals(resource);
        }
        public override int GetHashCode() => StringID.GetHashCode();
        public object Clone() => MemberwiseClone();
    }
}
