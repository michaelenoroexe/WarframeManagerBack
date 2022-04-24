using MongoDB.Bson;
namespace API.Models
{
    public class User
    {
        public ObjectId Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
