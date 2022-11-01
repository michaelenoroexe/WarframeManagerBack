namespace API.Models.UserWork
{
    /// <summary>
    /// Projection of user delete request data.
    /// </summary>
    public sealed class ReceivingPassword
    {
        /// <summary>
        /// Password of the user to be deletedю
        /// </summary>
        public string Password { get; init; }

        public ReceivingPassword(string password) => Password = password;
    }
}
