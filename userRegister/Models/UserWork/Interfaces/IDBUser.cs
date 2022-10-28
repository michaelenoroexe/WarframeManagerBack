using MongoDB.Bson;

namespace API.Models.UserWork.Interfaces
{
    internal interface IDBUser
    {
        ObjectId UserId { get; }
    }
}
