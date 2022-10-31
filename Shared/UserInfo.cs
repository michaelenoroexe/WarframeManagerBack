using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Shared
{
    public sealed class UserInfo
    {
        [BsonElement("_id")]
        [BsonId]
        public ObjectId? Id { get; }
        [BsonElement("login")]
        public string? Login { get; }
        [BsonElement("rank")]
        public int Rank { get; }
        [BsonElement("img")]
        public int Image { get; }

        public UserInfo(IUser? us, int rank = 0, int image = 0)
        {
            Id = us?.Id;
            Login = us?.Login;
            Rank = rank;
            Image = image;
        }
    }
}
