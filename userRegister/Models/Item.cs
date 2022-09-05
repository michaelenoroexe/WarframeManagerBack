using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace API.Models
{
    public class Item : Resource
    {
        [BsonElement("createTime")]
        public int CreationTime { get; set; }
        [BsonElement("credits")]
        public int Credits { get; set; }
        [BsonElement("components")]
        public Dictionary<string, int> NeededResources { get; set; }
        // List<Resource>

        public Item() { }
        public Item(ObjectId id, string name, string[] type, int creationTime, int credits, Dictionary<string, int> neededRes, string[] location = null, bool mastery=false) : base(id, name, type, location, mastery)
        {
            CreationTime = creationTime;
            Credits = credits;
            NeededResources = neededRes;
        }
        
    }
}
