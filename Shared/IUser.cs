using MongoDB.Bson;

namespace Shared
{
    public interface IUser
    {
        public ObjectId Id { get; init; }

        public string Login { get; init; }
    }
}
