using Shared;

namespace API.Repositories
{
    /// <summary>
    /// Represent object that porecc user change data requests.
    /// </summary>
    public interface IChangeData
    {
        /// <summary>
        /// Update value of user resource number.
        /// </summary>
        /// <param name="user">User whose resource needed to update.</param>
        /// <param name="res">Resource id / new number.</param>
        public void UpdateResource(IUser user, KeyValuePair<string, int> res);
        /// <summary>
        /// Update value of user item number.
        /// </summary>
        /// <param name="user">User whose item needed to update.</param>
        /// <param name="item">Item id / new number.</param>
        public void UpdateItem(IUser user, KeyValuePair<string, int> item);
        /// <summary>
        /// Update value of user Credits number.
        /// </summary>
        /// <param name="user">User whose credits needed to update.</param>
        /// <param name="credits">New credits number.</param>
        public void UpdateCredits(IUser user, int credits);
        /// <summary>
        /// Update value of user profile.
        /// </summary>
        /// <param name="user">User whose profile needed to update.</param>
        /// <param name="userInfo">New user profile information.</param>
        public void UpdateProfile(IUser user, UserInfo userInfo);
    }
}
