namespace UserValidation.LoginPasswordValidator
{
    internal sealed class LogPassUserConverter : IUserConverter<(string Login, string Password)>
    {
        public IClientUser CreateUser((string Login, string Password) user)
            => new ClientUser(user.Login, user.Password);
    }
}
