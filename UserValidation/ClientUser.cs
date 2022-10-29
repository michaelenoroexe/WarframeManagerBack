namespace UserValidation
{
    public class ClientUser : IClientUser
    {
        public string Login { get; init; }

        public string? Password { get; init; }

        public ClientUser(string login, string? password = null)
        {
            Login = login;
            Password = password;
        }
    }
}
