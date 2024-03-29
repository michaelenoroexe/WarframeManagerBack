﻿using BufferUserRequests.ChangeManagers;
using BufferUserRequests.DBSaver;
using Microsoft.Extensions.Logging;
using Shared;

namespace BufferUserRequests.ManStorage
{
    internal sealed class ManagersStorage : IManagersStorage
    {
        private readonly Saver _saver;
        private readonly IUser _user;
        private readonly ILogger _logger;
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
        /// Try get credit manager.
        /// </summary>
        /// <returns>Manager if it instantiate or null if user dont have current type of changes.</returns>
        public ISavableChangeManager<int, UserResources>? TryGetCreditManager() => _creditManager;
        /// <summary>
        /// Get manager to control changes.
        /// </summary>
        /// <returns>Credits changes manager.</returns>
        public ISavableChangeManager<int, UserResources> GetCreditManager() => _creditManager ??= new CreditChangeManager();
        /// <summary>
        /// Try get profile manager.
        /// </summary>
        /// <returns>Manager if it instantiate or null if user dont have current type of changes.</returns>
        public ISavableChangeManager<UserInfo, UserInfo>? TryGetProfileManager() => _profileManager;
        /// <summary>
        /// Get manager to control changes.
        /// </summary>
        /// <returns>Profile changes manager.</returns>
        public ISavableChangeManager<UserInfo, UserInfo> GetProfileManager() => _profileManager ??= new ProfileChangeManager(_user);
        /// <summary>
        /// Get manager to control changes.
        /// </summary>
        /// <returns>Items changes manager.</returns>
        public ISavableChangeManager<KeyValuePair<string, int>, Dictionary<string, int>, UserResources> GetItemsManager()
            => _itemManager ??= new ItemChangeManager();
        /// <summary>
        /// Try get item manager.
        /// </summary>
        /// <returns>Manager if it instantiate or null if user dont have current type of changes.</returns>
        public ISavableChangeManager<KeyValuePair<string, int>, Dictionary<string, int>, UserResources>? TryGetItemsManager() => _itemManager;
        /// <summary>
        /// Get manager to control changes.
        /// </summary>
        /// <returns>Resources changes manager.</returns>
        public ISavableChangeManager<KeyValuePair<string, int>, Dictionary<string, int>, UserResources> GetResourceManager()
            => _resManager ??= new ResourceChangeManager();
        /// <summary>
        /// Try get resource manager.
        /// </summary>
        /// <returns>Manager if it instantiate or null if user dont have current type of changes.</returns>
        public ISavableChangeManager<KeyValuePair<string, int>, Dictionary<string, int>, UserResources>? TryGetResourceManager() => _resManager;
        /// <summary>
        /// Save state in all managers to Database.
        /// </summary>
        public void Save()
        {
            _logger.LogInformation($"Saving '{_user}' changes to DB");
            // Save changes to User resources.
            if (_itemManager is not null || _resManager is not null || _creditManager is not null)
                _saver.Save(_user, _itemManager!, _resManager!, _creditManager!);
            // Save changes to user profile.
            if (_profileManager is not null)
                _saver.Save(_user, _profileManager);
        }
    }
}
