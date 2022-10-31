namespace API.Models.UserWork
{
    public class ReceivingChangePassword
    {
        public string OldPassword { get; init; }
        public string NewPassword { get; init; }

        public ReceivingChangePassword(string oldPassword, string newPassword)
        {
            OldPassword = oldPassword;
            NewPassword = newPassword;
        }
    }
}
