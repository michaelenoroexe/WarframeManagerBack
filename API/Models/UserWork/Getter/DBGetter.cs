using MongoDB.Driver;
using Shared;

namespace API.Models.UserWork.Getter
{
    /// <summary>
    /// Class to get data from database.
    /// </summary>
    internal abstract class DBGetter
    {
        protected readonly IMongoCollection<UserResources> _resourceCollection;
        protected readonly IMongoCollection<UserInfo> _profileCollection;

        protected DBGetter(IMongoCollection<UserResources> resourceCollection, IMongoCollection<UserInfo> profileCollection)
        {
            _resourceCollection = resourceCollection;
            _profileCollection = profileCollection;
        }
        /// <summary>
        /// Get item list of user from db.
        /// </summary>
        /// <param name="user">User whose items needed to get.</param>
        /// <returns>Dictionary with itemid/owned number, null if user dont have record in DB.</returns>
        public abstract Task<Dictionary<string, int>?> GetItems(IUser user);
        /// <summary>
        /// Get resource list of user from db.
        /// </summary>
        /// <param name="user">User whose resources needed to get.</param>
        /// <returns>Dictionary with resourceid/owned number, null if user dont have record in DB.</returns>
        public abstract Task<Dictionary<string, int>?> GetResources(IUser user);
        /// <summary>
        /// Get credit number of user from db.
        /// </summary>
        /// <param name="user">User whose credits needed to get.</param>
        /// <returns>Сredit number, null if user dont have record in DB.</returns>
        public abstract Task<int?> GetCredits(IUser user);
        /// <summary>
        /// Get profile info of user from db.
        /// </summary>
        /// <param name="user">User whose profile needed to get.</param>
        /// <returns>Users profile info, null if user dont have record in DB.</returns>
        public abstract Task<UserInfo?> GetProfile(IUser user);
    }
}
