using MongoDB.Bson;

namespace API.Models.Interfaces
{
    public interface IUser
    {
        public ObjectId Id { get; init; }

        public string Login { get; init; }
    }
}
