namespace API.Models.UserWork
{
    public sealed class ReceivingChangePassword
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
