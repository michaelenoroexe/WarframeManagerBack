using API.Models;
using API.Models.Responses;
using Microsoft.Extensions.Options;
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
//_jwtAuthentication = jwtAuthentication ?? throw new ArgumentNullException(nameof(jwtAuthentication));
        }
        // Function that confirm validation of user input data.
        public bool DataValidation(string data)
        {
            try
            {
                string validsymb = @"1234567890qwertyuiopasdfghjklzxcvbnm!#$%&()*+,-./;<=>?@[\]^_{|}~";
                string datalower = data.ToLower();
                if (!char.IsLetter(datalower[0])) return false;
                if (datalower.Except(validsymb).Count() > 0) return false;
                if (datalower.Length < 4 || datalower.Length > 32) return false;
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        // Finding single user, when 0 = null, if users more than one = exeption.
        public async Task<User> FindUserAsync(string us)
        {
            return await _userCollection
                .Find(new BsonDocument("Login", us))
                .SingleOrDefaultAsync();
        }
        // Async function that valid and add user into database.
        public async Task<UserResponse> AddUserAsync(User us)
        {
            try
            {
                User? user = await FindUserAsync(us.Login);
                if (user != null) throw new Exception("Already in database");
                user = new User();
                user.Login = us.Login;
                user.Password = Hash.HashString(us.Password);
                await _userCollection.WithWriteConcern(new WriteConcern(1)).InsertOneAsync(user);
                return new UserResponse(user);
                throw new Exception("Unknown Error");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                if (ex.Message == "Already in database")
                    return new UserResponse(false, "A user with the given login already exists.");
                 return new UserResponse(false, ex.Message);
            }
        }
        // Async function that valid and sign user in.
        public async Task<UserResponse> LoginUserAsync(User us)
        {
            try
            {
                User? user = await FindUserAsync(us.Login);
                if (user == null || !Hash.Verify(us.Password, user.Password)) throw new Exception("Wrong Login or Password!");

                return new UserResponse(true, JwtAuthentication.GenerateToken(user));
                throw new Exception("Unknown Error");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                if (ex.Message == "Already in database")
                    return new UserResponse(false, "A user with the given login already exists.");
                return new UserResponse(false, ex.Message);
            }
        }

    }
}
