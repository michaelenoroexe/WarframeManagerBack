using MongoDB.Bson;

namespace Shared
{
    /// <summary>
    /// Represent object with user information used by application.
    /// </summary>
    public interface IUser : IEquatable<IUser>
    {
        /// <summary>
        /// User database id.
        /// </summary>
        public ObjectId Id { get; init; }
        /// <summary>
        /// User login.
        /// </summary>
        public string Login { get; init; }
    }
}
