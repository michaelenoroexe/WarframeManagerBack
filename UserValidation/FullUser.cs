using MongoDB.Bson;
using Shared;

namespace UserValidation
{
    public sealed class FullUser : IUser, IClientUser
    {
        public ObjectId Id { get; init; }
        public string Login { get; init; }
        public string Password { get; init; }

        public FullUser(ObjectId id, string login, string password)
        {
            Id = id;
            Login = login;
            Password = password;
        }

        public bool Equals(IUser? other) => other is not null && Id.Equals(other.Id);
    }
}
