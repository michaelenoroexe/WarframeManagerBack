﻿using BufferUserRequests.DBSaver;
using BufferUserRequests.ManStorage;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Shared;

namespace BufferUserRequests
{
    public class UsersChangeBuffer
    {
        private LinkedList<UserInfoChanges> _changeBuffer;
        private Saver _saver;
        private ManagersStorageBuilder _managersStorageBuilder;
        private ILogger _logger;
        /// <summary>
        /// Instantiate UserChanges buffer.
        /// </summary>
        public UsersChangeBuffer(IMongoCollection<UserResources> userResCollection,
                                 IMongoCollection<UserInfo> userProfCollection, ILogger<UsersChangeBuffer> loger)
        {
            _saver = new Saver(userResCollection, userProfCollection);
            _logger = loger;
            _managersStorageBuilder = new ManagersStorageBuilder(_saver, _logger);
            _changeBuffer = new LinkedList<UserInfoChanges>();
        }
        /// <summary>
        /// Get associated with user change buffer.
        /// </summary>
        /// <param name="user">User which buffer needed to find.</param>
        public UserInfoChanges GetUserChanges(IUser user)
        {
            // Return existed user info.
            UserInfoChanges? changes = _changeBuffer.FirstOrDefault(changes => changes.User.Equals(user));
            if (changes is not null) return changes;
            // Create new user info.
            var manager = _managersStorageBuilder.CreateManagersStorage(user);
            changes = new UserInfoChanges(manager, user, _changeBuffer);
            _changeBuffer.AddLast(changes);
            return changes;
        }
        /// <summary>
        /// Try to get associated with user change buffer.
        /// </summary>
        /// <param name="user">User which buffer needed to find.</param>
        /// <return>Return associated with user buffer, if user dont have buffer return null.</return>
        public UserInfoChanges? TryGetUserChanges(IUser user)
            => _changeBuffer.FirstOrDefault(changes => changes.User.Equals(user));

    }
}