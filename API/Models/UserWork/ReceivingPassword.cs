namespace API.Models.UserWork
{
    public sealed class ReceivingPassword
    {
        public string Password { get; init; }

        public ReceivingPassword(string password) => Password = password;
    }
}
