using API.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using API.Models.Interfaces;
using System.Diagnostics.CodeAnalysis;

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
        public bool Equals(IResource? x, IResource? y)
        {
            return x?.StringID.GetHashCode() == y?.StringID.GetHashCode();
        }
        public int GetHashCode([DisallowNull] IResource obj)
        {
            return obj.StringID.GetHashCode();
        }
    }
}
