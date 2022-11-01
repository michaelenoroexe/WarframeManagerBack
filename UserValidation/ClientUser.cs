namespace UserValidation
{
    internal sealed class ClientUser : IClientUser
    {
        /// <summary>
        /// Users login.
        /// </summary>
        public string Login { get; init; }
        /// <summary>
        /// Users password.
        /// </summary>
        public string? Password { get; init; }

        public ClientUser(string login, string? password = null)
        {
            Login = login;
            Password = password;
        }
    }
}
