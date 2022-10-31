using MongoDB.Bson;

namespace Shared
{
    // Model of user information
    public class User : IUser
    {
        public ObjectId Id { get; init; }
        public string Login { get; init; }

        public User(ObjectId id, string login)
        {
            Id = id;
            Login = login;
        }

        public bool Equals(IUser? other) => other is not null && Id.Equals(other.Id);
    }
}
