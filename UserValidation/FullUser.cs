using MongoDB.Bson;
using Shared;

namespace UserValidation
{
    /// <summary>
    /// Full user information.
    /// </summary>
    public sealed class FullUser : IUser, IClientUser
    {
        /// <summary>
        /// Users database id.
        /// </summary>
        public ObjectId Id { get; init; }
        /// <summary>
        /// Users login.
        /// </summary>
        public string Login { get; init; }
        /// <summary>
        /// Users password.
        /// </summary>
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
