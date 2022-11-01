using Shared;

namespace BufferUserRequests
{
    /// <summary>
    /// Represent manages of users changers storages. 
    /// </summary>
    public interface IManagersStorage
    {
        /// <summary>
        /// Return credit manager if it exists otherwise null.
        /// </summary>
        public ISavableChangeManager<int, UserResources>? TryGetCreditManager();
        /// <summary>
        /// Return credit manager if it exists otherwise create and return.
        /// </summary>
        public ISavableChangeManager<int, UserResources> GetCreditManager();
        /// <summary>
        /// Return profile manager if it exists otherwise null.
        /// </summary>
        public ISavableChangeManager<UserInfo, UserInfo>? TryGetProfileManager();
        /// <summary>
        /// Return profile manager if it exists otherwise create and return.
        /// </summary>
        public ISavableChangeManager<UserInfo, UserInfo> GetProfileManager();
        /// <summary>
        /// Return item manager if it exists otherwise create and return.
        /// </summary>
        public ISavableChangeManager<KeyValuePair<string, int>, Dictionary<string, int>, UserResources> GetItemsManager();
        /// <summary>
        /// Return items manager if it exists otherwise null.
        /// </summary>
        public ISavableChangeManager<KeyValuePair<string, int>, Dictionary<string, int>, UserResources>? TryGetItemsManager();
        /// <summary>
        /// Return resource manager if it exists otherwise create and return.
        /// </summary>
        public ISavableChangeManager<KeyValuePair<string, int>, Dictionary<string, int>, UserResources> GetResourceManager();
        /// <summary>
        /// Return resource manager if it exists otherwise null.
        /// </summary>
        public ISavableChangeManager<KeyValuePair<string, int>, Dictionary<string, int>, UserResources>? TryGetResourceManager();
        public void Save();
    }
}
