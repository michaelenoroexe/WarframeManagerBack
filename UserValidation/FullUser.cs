using MongoDB.Bson;
using Shared;

namespace UserValidation
{
    public class FullUser : User, IClientUser
    {
        public string Password { get; init; }

        public FullUser(ObjectId id, string login, string password) 
            : base(id, login) => Password = password;
    }
}
