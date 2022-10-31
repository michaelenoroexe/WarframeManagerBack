using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace API.Models.Common
{
    public class Restype
    {
        [BsonIgnore]
        private ObjectId _id;

        [BsonElement("_id")]
        [BsonId]
        public ObjectId Id
        {
            get
            {
                return _id;
            }
            set
            {
                strId = value.ToString();
                _id = value;
            }
        }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonIgnore]
        public string strId { get; set; }
    }
}
