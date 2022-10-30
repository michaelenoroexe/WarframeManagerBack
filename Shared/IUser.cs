using MongoDB.Bson;

namespace Shared
{
    public interface IUser : IEquatable<IUser>
    {
        public ObjectId Id { get; init; }

        public string Login { get; init; }
    }
}
