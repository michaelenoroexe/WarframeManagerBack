using API.Models;
using API.Repositories;
using MongoDB.Driver;
using MongoDB.Bson;

namespace API.Repositories
{
    public class UserRepository
    {
        private readonly IMongoCollection<User> _userCollection;

        public UserRepository()
        {
            _userCollection = DBClient.db.GetCollection<User>("Users");
        }

        //Checking database for existing usename
        public bool UserCheck(string us)
        {
            BsonDocument userlogin = new Dictionary<string, string>() { { "Login", us } }.ToBsonDocument();
            List<User> ans = _userCollection.Find(userlogin).Limit(1).ToList();
            if (ans.Count == 0) return true;
            if (ans.Count == 1) return false;
            if (ans.Count > 1 | ans.Count < 0) throw new IndexOutOfRangeException();
            return false;
        }
    }
}
