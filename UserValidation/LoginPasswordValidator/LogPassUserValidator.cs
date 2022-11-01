using MongoDB.Driver;
using Shared;
using UserValidation.DBSearcher;

namespace UserValidation.LoginPasswordValidator
{
    /// <summary>
    /// Validate user by login password authentication system.
    /// </summary>
    public sealed class LogPassUserValidator : IUserValidator<(string Login, string Password)>
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
        /// <summary>
        /// Get converter to transform inputed data to user info.
        /// </summary>
        /// <returns>Login password converter.</returns>
        public IUserConverter<(string Login, string Password)> GetConverter() => new LogPassUserConverter();
        /// <summary>
        /// Search user inputed string on unappropriate symbols.
        /// </summary>
        /// <param name="value">Inputed string.</param>
        /// <returns>True if string is valid, otherwise false</returns>
        public bool ValidateCredential(string value) => StringValidator.GetStringValidator().Validate(value);
        /// <summary>
        /// Validate user information.
        /// </summary>
        /// <param name="user">User information.</param>
        /// <returns>Application ready to work user information if user already exists in DB, otherwise null.</returns>
        /// <exception cref="ArgumentNullException">If user dont have login or password.</exception>
        /// <exception cref="ArgumentException">If users login or password contains unapropriate symbols.</exception>
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
