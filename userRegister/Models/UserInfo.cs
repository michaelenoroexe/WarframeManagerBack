using API.Models.UserWork;
using API.Models.UserWork.Interfaces;
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
        public string Login { get; set; }
        [BsonElement("rank")]
        public int Rank { get; set; }
        [BsonElement("img")]
        public int Image { get; set; }

        public UserInfo(IUser us, int rank, int image)
        {
            Id = us?.Id;
            Login = us?.Login;
            Rank = rank;
            Image = image;
        }
        public UserInfo()
        {

        }
        public UserInfo(string login, int rank, int image)
        {
            Login = login;
            Rank = rank;
            Image = image;
        }
        //public UserInfo WithoutId()
        //{
        //    return new UserInfo(this.Login, this.Rank, this.Image);
        //}
    }
}
