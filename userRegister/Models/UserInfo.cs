using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace API.Models
{
    public class UserInfo
    {
        [BsonElement("_id")]
        [BsonId]
        public ObjectId? Id { get; set; }
        [BsonElement("login")]
        public string? Login { get; init; }
        [BsonElement("rank")]
        public int Rank { get; init; }
        [BsonElement("img")]
        public int Image { get; init; }

        public UserInfo(User us, int rank, int image)
        {
            Id = us.Id;
            Login = us.Login;
            Rank = rank;
            Image = image;
        }
    }
}
