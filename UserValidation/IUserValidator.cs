using Shared;

namespace UserValidation
{
    public interface IUserValidator<T>
    {
        /// <summary>
        /// Validate and find user.
        /// </summary>
        /// <param name="user">User to validate.</param>
        /// <returns>Return user if he correct and in database, null if user correct but not in db, else exeption.</returns>
        public IUser? ValidateUser(IClientUser user);

        public bool ValidateCredential(string value); 

        public IUserConverter<T> GetConverter();
    }
}
