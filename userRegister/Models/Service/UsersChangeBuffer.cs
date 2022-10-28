using API.Models.UserWork.Changes;
using API.Models.UserWork.Interfaces;

namespace API.Models.Service
{
    internal class UsersChangeBuffer
    {
        private Dictionary<IUser, UserInfoChanges> _changeBuffer;
        private Saver _saver;
        private ManagersStorageBuilder _managersStorageBuilder;
        private ILogger _logger;
        /// <summary>
        /// Instantiate UserChanges buffer.
        /// </summary>
        public UsersChangeBuffer(Saver saver, ILogger loger, ManagersStorageBuilder manager)
        {           
            _saver = saver;
            _logger = loger;
            _managersStorageBuilder = manager;
            _changeBuffer = new Dictionary<IUser, UserInfoChanges>();
        }
        public UserInfoChanges GetUserChanges(IUser user)
        {
            UserInfoChanges? changes;
            if (_changeBuffer.TryGetValue(user, out changes)) return changes;

            var manager = _managersStorageBuilder.CreateManagersStorage(user);
            changes = new UserInfoChanges(manager);
            _changeBuffer.Add(user, changes);
            return changes;
        }
    }
}
