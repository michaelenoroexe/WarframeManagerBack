namespace API.Models.UserWork
{
    public class ReceivingPassword
    {
        public string Password { get; init; }

        public ReceivingPassword(string password)
        {
            Password = password;
        }
    }
}
