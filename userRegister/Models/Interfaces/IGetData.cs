namespace API.Models.Interfaces
{
    public interface IGetData
    {    
        #region Items
        public Task<List<Item>> GetUserItemsAsync();
        /// <summary>
        /// Get all item list.
        /// </summary>
        /// <returns></returns>
        public Task<List<Item>> GetItemsListAsync();
        /// <summary>
        /// Get all user items.
        /// </summary>
        /// <param name="userId">UserId which items needed to get.</param>
        /// <returns>Dictionary of item name as key, and item number as value. Null if user dont have any.</returns>
        public Task<Dictionary<string, int>?> GetUsersItemsAsync(IUser user);
        #endregion

        #region Resources
        public Task<List<Item>> GetUserResourcesAsync();
        /// <summary>
        /// Get all resource list.
        /// </summary>
        public Task<List<Item>> GetResourcesListAsync();
        /// <summary>
        /// Get all user resources.
        /// </summary>
        /// <param name="userId">UserId which resources needed to get.</param>
        /// <returns>Dictionary of resource name as key, and resource number as value.  Null if user dont have any.</returns>
        public Task<Dictionary<string, int>?> GetUsersResourcesAsync(IUser user);
        #endregion

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
    }
}
