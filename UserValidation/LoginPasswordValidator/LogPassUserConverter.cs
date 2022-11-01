namespace UserValidation.LoginPasswordValidator
{
    /// <summary>
    /// Create client user from login password.
    /// </summary>
    internal sealed class LogPassUserConverter : IUserConverter<(string Login, string Password)>
    {
        /// <summary>
        /// Create user for application work.
        /// </summary>
        /// <param name="user">Users login and password.</param>
        /// <returns>Client user.</returns>
        public IClientUser CreateUser((string Login, string Password) user)
            => new ClientUser(user.Login, user.Password);
    }
}
