namespace API.Models.UserWork
{
    /// <summary>
    /// Projection of user password change request data.
    /// </summary>
    public sealed class ReceivingChangePassword
    {
        /// <summary>
        /// Old user password stored in DB.
        /// </summary>
        public string OldPassword { get; init; }
        /// <summary>
        /// New users wanted password.
        /// </summary>
        public string NewPassword { get; init; }

        public ReceivingChangePassword(string oldPassword, string newPassword)
        {
            OldPassword = oldPassword;
            NewPassword = newPassword;
        }
    }
}
