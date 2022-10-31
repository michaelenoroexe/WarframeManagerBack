using MongoDB.Driver;
using Shared;
using UserValidation.DBSearcher;

namespace UserValidation.LoginPasswordValidator
{
    public class LogPassUserValidator : IUserValidator<(string Login, string Password)>
    {
        private readonly DBUserSearcher _dbSearcher;
        private readonly IPasswordEqualityComparer _comparer;
        /// <summary>
        /// Create validator based on login and password.
        /// </summary>
        /// <param name="comparer">Comparer to compare inputed password with stored.</param>
        public LogPassUserValidator(IPasswordEqualityComparer comparer, IMongoCollection<FullUser> userCollection)
        {
            _comparer = comparer;
            _dbSearcher = new DBUserSearcher(userCollection);
        }
        public IUserConverter<(string Login, string Password)> GetConverter()
        {
            return new LogPassUserConverter();
        }

        public bool ValidateCredential(string value)
        {         
            return StringValidator.GetStringValidator().Validate(value);
        }

        public IUser? ValidateUser(IClientUser user)
        {
            var inputValidator = StringValidator.GetStringValidator();
            if (user?.Login is null) throw new ArgumentNullException("User dont have login.");
            if (user?.Password is null) throw new ArgumentNullException("User dont have Password.");
            if (!inputValidator.Validate(user.Login)) throw new ArgumentException("Login is incorrect.");
            if (!inputValidator.Validate(user.Password)) throw new ArgumentException("Password is incorrect.");

            FullUser? userInDB = _dbSearcher.TryFindUserAsync(user).Result;
            if (userInDB is null) return null;

            if (!_comparer.Equals(user.Password, userInDB.Password)) throw new ArgumentException("Password don't match.");
            return userInDB;
        }
    }
}
