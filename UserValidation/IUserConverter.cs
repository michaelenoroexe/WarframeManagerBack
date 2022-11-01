namespace UserValidation
{
    /// <summary>
    /// Converted user inputed information to correct object with users info.
    /// </summary>
    /// <typeparam name="T">ClaimsPrincipal to JWT authentication, (string, string) to Login password authentication.</typeparam>
    public interface IUserConverter<T>
    {
        /// <summary>
        /// Create object of user information.
        /// </summary>
        /// <param name="user">Information about user.</param>
        /// <returns>Representation of user information.</returns>
        public IClientUser CreateUser(T user);
    }
}
