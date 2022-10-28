using API.Models.Interfaces;
using API.Models.Service;
using API.Models.UserWork.Interfaces;
using MongoDB.Driver;

namespace API.Models.UserWork.Changes
{
    internal class ManagersStorage : IManagersStorage
    {
        private Saver _saver;
        private IUser _user;
        private ILogger _logger;
        #region Managers instances
        private ResourceChangeManager? _resManager;
        private ItemChangeManager? _itemManager;
        private CreditChangeManager? _creditManager;
        private ProfileChangeManager? _profileManager;
        #endregion
        /// <summary>
        /// Create managerStorage with collections.
        /// </summary>
        public ManagersStorage(IUser user, Saver saver, ILogger loger)
        {
            _user = user;
            _saver = saver;
            _logger = loger;
        }
        /// <summary>
        /// Get manager to control changes.
        /// </summary>
        /// <returns>Credits changes manager.</returns>
        public ISavableChangeManager<int, UserResources> GetCreditManager()
        {
            if (_creditManager is not null) return _creditManager;
            _creditManager = new CreditChangeManager();
            return _creditManager;
        }
        /// <summary>
        /// Get manager to control changes.
        /// </summary>
        /// <returns>Items changes manager.</returns>
        public ISavableChangeManager<KeyValuePair<IResource, int>, Dictionary<IResource, int>, UserResources> GetItemsManager()
        {
            if (_itemManager is not null) return _itemManager;
            _itemManager = new ItemChangeManager();
            return _itemManager;
        }
        /// <summary>
        /// Get manager to control changes.
        /// </summary>
        /// <returns>Profile changes manager.</returns>
        public ISavableChangeManager<UserInfo, UserInfo> GetProfileManager()
        {
            if (_profileManager is not null) return _profileManager;
            _profileManager = new ProfileChangeManager();
            return _profileManager;
        }
        /// <summary>
        /// Get manager to control changes.
        /// </summary>
        /// <returns>Resources changes manager.</returns>
        public ISavableChangeManager<KeyValuePair<IResource, int>, Dictionary<IResource, int>, UserResources> GetResourceManager()
        {
            if (_resManager is not null) return _resManager;
            _resManager = new ResourceChangeManager();
            return _resManager;
        }
        /// <summary>
        /// Save state in all managers to Database.
        /// </summary>
        public void Save()
        {
            _logger.LogInformation($"Saving '{_user}' changes to DB");
            // Save changes to User resources.
            if (_itemManager is not null || _profileManager is not null || _creditManager is not null)
                _saver.Save(_user, _itemManager!, _resManager!, _creditManager!);
            // Save changes to user profile.
            if (_profileManager is not null)
                _saver.Save(_user, _profileManager);
        }
    }
}
