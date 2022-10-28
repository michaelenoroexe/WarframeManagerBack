using API.Models.Interfaces;
using API.Models.UserWork.Interfaces;
using MongoDB.Bson;

namespace API.Models.UserWork
{
    // Model of user information
    public class User : IUser
    {
        public ObjectId Id { get; init; }
        public string Login { get; init; }
        public string? Password { get; init; }

        public User(ObjectId id, string login, string? password = null)
        {
            Id = id;
            Login = login;
            Password = password;
        }
    }
}
