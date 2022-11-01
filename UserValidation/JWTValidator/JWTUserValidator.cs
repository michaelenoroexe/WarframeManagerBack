using MongoDB.Driver;
using Shared;
using System.Security.Claims;
using UserValidation.DBSearcher;

namespace UserValidation.JWTValidation
{
    public sealed class JWTUserValidator : IUserValidator<ClaimsPrincipal>
    {
        private readonly DBUserSearcher _dbSearcher;

        public JWTUserValidator(IMongoCollection<FullUser> userCollection)
            => _dbSearcher = new DBUserSearcher(userCollection);
        /// <summary>
        /// Creates converter that converts user intput data to type ready to validate.
        /// </summary>
        /// <returns></returns>
        public IUserConverter<ClaimsPrincipal> GetConverter()
            => new JWTUserConverter();

        public bool ValidateCredential(string value)
            => StringValidator.GetStringValidator().Validate(value);

        public IUser? ValidateUser(IClientUser user)
        {
            if (user?.Login is null) throw new ArgumentNullException("User dont have login.");
            if (!StringValidator.GetStringValidator().Validate(user.Login)) throw new ArgumentException("Login is incorrect.");
            return _dbSearcher.TryFindUserAsync(user).Result;
        }
    }
}
