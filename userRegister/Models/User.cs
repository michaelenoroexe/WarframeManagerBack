using MongoDB.Bson;
namespace API.Models
{
    // Model of user information
    public class User
    {
        public ObjectId Id { get; init; }
        public string Login { get; init; }
        public string Password { get; init; }
    }
}
