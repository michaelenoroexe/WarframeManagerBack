using Shared;

namespace API.Models.UserWork.Interfaces
{
    /// <summary>
    /// Represent object that gets user information.
    /// </summary>
    internal interface IUserInfoGetter
    {
        /// <summary>
        /// Return full users dictionary of item id / item owned.
        /// </summary>
        /// <param name="user">User whose items needed to find.</param>
        Task<Dictionary<string, int>?> GetFullItemAsync(IUser user);
        /// <summary>
        /// Return full users dictionary of resource id / resource owned.
        /// </summary>
        /// <param name="user">User whose resource needed to find.</param>
        Task<Dictionary<string, int>?> GetFullResourceAsync(IUser user);
        /// <summary>
        /// Get user credits number.
        /// </summary>
        /// <param name="user">User whose credits number needed to find.</param>
        Task<int> GetCreditsAsync(IUser user);
        /// <summary>
        /// Get user profile info.
        /// </summary>
        /// <param name="user">User whose profile info needed to find.</param>
        Task<UserInfo> GetProfileAsync(IUser user);
    }
}
