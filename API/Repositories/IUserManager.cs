using Shared;

namespace API.Repositories
{
    /// <summary>
    /// Represent object processing user administration requests.
    /// </summary>
    public interface IUserManager
    {
        /// <summary>
        /// Add user to database.
        /// </summary>
        /// <param name="user">User info that needed to add to DB.</param>
        /// <param name="password">Password of user added to DB.</param>
        public Task AddUserAsync(IUser user, string password);
        /// <summary>
        /// Sign user in.
        /// </summary>
        /// <param name="user">User info from DB.</param>
        /// <returns>JWT token generated from inputed user info.</returns>
        public string SignInUser(IUser user);
        /// <summary>
        /// Change user password.
        /// </summary>
        /// <param name="user">User info.</param>
        /// <param name="newPassword">New user password.</param>
        public Task ChangeUserPasswordAsync(IUser user, string newPassword);
        /// <summary>
        /// Delete user from database.
        /// </summary>
        /// <param name="user">User that needed to delete.</param>
        public Task DeleteUserAsync(IUser user);
    }
}
