using MongoDB.Bson;
namespace API.Models
{
    // Model of user information
    public class User
    {
        public ObjectId Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
