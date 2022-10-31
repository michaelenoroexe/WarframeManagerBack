using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace API.Models.Common.ItemComp
{
    public class Resource : IResource
    {
        [BsonElement("_id")]
        [JsonProperty("_id")]
        [BsonId]
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
        private int _owned;
        [BsonIgnore]
        public int Owned { get => _owned; }
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
        public void SetOwned(int number)
        {
            if (number >= 0) _owned = number;
        }
        public bool Equals(IResource? other)
        {
            if (other == null) return false;
            return GetHashCode() == other?.GetHashCode();
        }
        public override bool Equals(object? obj)
        {
            if (obj is string) return StringID.Equals((string)obj, StringComparison.OrdinalIgnoreCase);
            if (obj is not IResource) return false;
            return Equals((IResource)obj);
        }
        public override int GetHashCode()
        {
            return StringID.GetHashCode();
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
