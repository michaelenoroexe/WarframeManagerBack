using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace Shared
{
    /// <summary>
    /// Projection class for users profile information.
    /// </summary>
    public sealed class UserInfo
    {
        /// <summary>
        /// Users database id.
        /// </summary>
        [BsonId]
        [BsonElement("_id")]       
        public ObjectId? Id { get; }
        /// <summary>
        /// User unique login.
        /// </summary>
        [BsonElement("login")]
        public string? Login { get; }
        /// <summary>
        /// User rank.
        /// </summary>
        [BsonElement("rank")]
        public int Rank { get; }
        /// <summary>
        /// User profile image.
        /// </summary>
        [BsonElement("img")]
        public int Image { get; }
        [BsonConstructor]
        [JsonConstructor]
        public UserInfo(ObjectId? id, string? login, int rank, int image)
        {
            Id = id;
            Login = login;
            Rank = rank;
            Image = image;
        }
        public UserInfo(IUser? us, int rank = 0, int image = 0)
        {
            Id = us?.Id;
            Login = us?.Login;
            Rank = rank;
            Image = image;
        }
    }
}
