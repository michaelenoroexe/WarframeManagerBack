using API.Models;
using API.Repositories;
using MongoDB.Driver;
using MongoDB.Bson;

namespace API.Repositories
{
    public class UserRepository
    {
        public readonly IMongoCollection<User> _userCollection;
        public UserRepository()
        {
            _userCollection = DBClient.db.GetCollection<User>("Users");
        }

        public async Task<bool> DataValidationAsync(string data)
        {
            string validsymb = @"1234567890qwertyuiopasdfghjklzxcvbnm!#$%&()*+,-./;<=>?@[\]^_{|}~";
            string datalower = data.ToLower();
            if (datalower.Except(validsymb).Count() > 0) return false;
            if (datalower.Length < 4 || datalower.Length >32) return false;
            return true;
        }

        //Checking database for existing usename
        public bool UserCheck(string us)
        {
            BsonDocument userlogin = new Dictionary<string, string>() { { "Login", us } }.ToBsonDocument();
            int ans = _userCollection.Find(userlogin).Limit(1).ToList().Count;
            if (ans == 0) return true;
            if (ans == 1) return false;
            if (ans > 1 | ans < 0) throw new IndexOutOfRangeException();
            return false;
        }
    }
}
