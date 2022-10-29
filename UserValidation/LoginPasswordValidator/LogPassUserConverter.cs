namespace UserValidation.LoginPasswordValidator
{
    public class LogPassUserConverter : IUserConverter<(string Login, string Password)>
    {
        public IClientUser CreateUser((string Login, string Password) user)
        {
            return new ClientUser(user.Login, user.Password);
        }
    }
}
