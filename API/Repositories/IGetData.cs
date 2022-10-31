using API.Models.Common;
using API.Models.Common.ItemComp;
using Shared;

namespace API.Models.Interfaces
{
    public interface IGetData
    {
        #region For all
        /// <summary>
        /// Get all resource list.
        /// </summary>
        public IEnumerable<IResource> GetResourcesList();
        /// <summary>
        /// Get all item list.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IResource> GetItemsList();
        /// <summary>
        /// Get dictionary with all planets.
        /// </summary>
        /// <returns>All planets id/name.</returns>
        public Task<Dictionary<string, string>> GetPlanetListAsync();
        /// <summary>
        /// Get availible types of items. 
        /// </summary>
        /// <returns></returns>
        public Task<List<Restype>> GetTypesListAsync();
        #endregion

        #region User belong
        /// <summary>
        /// Get all resources, to users resources set having number.
        /// </summary>
        /// <param name="user">UserId which resources needed to get.</param>
        /// <returns>Dictionary of resource name as key, and resource number as value.</returns>
        public Task<IEnumerable<IResource>> GetUserResourcesAsync(IUser user);
        /// <summary>
        /// Get all items, to users items set having number.
        /// </summary>
        /// <param name="user">UserId which items needed to get.</param>
        /// <returns>Dictionary of item name as key, and item number as value.</returns>
        public Task<IEnumerable<IResource>> GetUserItemsAsync(IUser user);
        /// <summary>
        /// Get number of user credits.
        /// </summary>
        /// <param name="user">Which user get number.</param>
        /// <returns>Credits number.</returns>
        public Task<int> GetUserCreditsAsync(IUser user);
        /// <summary>
        /// Get info of user profile.
        /// </summary>
        /// <param name="user">User to find profile information.</param>
        /// <returns>Info about user profile.</returns>
        public Task<UserInfo> GetUserInfoAsync(IUser user);
        #endregion
    }
}
