using Shared;

namespace BufferUserRequests.ChangeManagers
{
    /// <summary>
    /// Manager of users profile info.
    /// </summary>
    internal sealed class ProfileChangeManager : ISavableChangeManager<UserInfo, UserInfo>
    {
        /// <summary>
        /// Storage of user changes.
        /// </summary>
        private UserInfo _storage;
        /// <summary>
        /// Get instance of ProfileChangeManager.
        /// </summary>
        public ProfileChangeManager(IUser user) => _storage = new UserInfo(user);
        /// <summary>
        /// Change storage to input value.
        /// </summary>
        public void Edit(UserInfo item) => _storage = item;
        /// <summary>
        /// Get current state of storage.
        /// </summary>
        public UserInfo GetCurrent() => _storage;
        /// <summary>
        /// Save state to object in argument.
        /// </summary>
        /// <param name="save">User resource item to store resource in.</param>
        public void Save(ref UserInfo save) => save = _storage;
    }
}
