using Shared;

namespace UserValidation
{
    /// <summary>
    /// Represent user validator.
    /// </summary>
    /// <typeparam name="T">ClaimsPrincipal to JWT authentication, (string, string) to Login password authentication.</typeparam>
    public interface IUserValidator<T>
    {
        /// <summary>
        /// Validate and find user.
        /// </summary>
        /// <param name="user">User to validate.</param>
        /// <returns>Return user if he correct and in database, null if user correct but not in db, else exeption.</returns>
        public IUser? ValidateUser(IClientUser user);
        /// <summary>
        /// Validate user inputed string.
        /// </summary>
        /// <param name="value">String needed to validate.</param>
        /// <returns>Binary result of validation.</returns>
        public bool ValidateCredential(string value);
        /// <summary>
        /// Return data converter.
        /// </summary>
        /// <returns>Converter.</returns>
        public IUserConverter<T> GetConverter();
    }
}
