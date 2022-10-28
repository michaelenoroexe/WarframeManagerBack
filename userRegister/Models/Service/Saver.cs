using API.Models.UserWork.Changes;
using API.Models.UserWork.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace API.Models.Service
{
    /// <summary>
    /// Class to save manager state to db.
    /// </summary>
    internal class Saver
    {
        private IMongoCollection<UserResources> _userResourcesCollection;
        private IMongoCollection<UserInfo> _userProfileInfoCollection;
        #region Get state from db
        /// <summary>
        /// Get current user resources.
        /// </summary>
        /// <param name="user">Current user.</param>
        private async Task<UserResources> GetCurrentResources(IUser user)
        {
            IAsyncCursor<UserResources> stat = await _userResourcesCollection.FindAsync(Builders<UserResources>.Filter.Eq(dbIt => dbIt.User, user.Id));
            UserResources result = await stat.SingleOrDefaultAsync();
            return result ?? new UserResources(user.Id);
        }
        /// <summary>
        /// Get current user porfile info.
        /// </summary>
        /// <param name="user">Current user.</param>
        /// <returns></returns>
        private async Task<UserInfo> GetCurrentProfile(IUser user)
        {
            IAsyncCursor<UserInfo> stat = await _userProfileInfoCollection.FindAsync(Builders<UserInfo>.Filter.Eq(dbIt => dbIt.Id, user.Id));
            UserInfo result = await stat.SingleOrDefaultAsync();
            return result ?? new UserInfo();
        }
        #endregion
        #region Set state in db
        /// <summary>
        /// Set current user resources.
        /// </summary>
        private async Task SetCurrentResources(UserResources resources)
        {
            if (resources.Id is not null)
            {
                await _userResourcesCollection.UpdateOneAsync(Builders<UserResources>.Filter.Eq(dbIt => dbIt.User, resources.User),
                                                                     Builders<UserResources>.Update.Set(x => x, resources));
                return;
            }
            resources.Id = ObjectId.GenerateNewId();
            await _userResourcesCollection.InsertOneAsync(resources);
        }
        /// <summary>
        /// Set current user porfile info.
        /// </summary>
        private async Task SetCurrentProfile(UserInfo profile, IUser user)
        {
            if (profile.Id is not null)
            {
                await _userProfileInfoCollection.UpdateOneAsync(Builders<UserInfo>.Filter.Eq(dbIt => dbIt.Id, profile.Id),
                                                                       Builders<UserInfo>.Update.Set(x => x, profile));
                return;
            }
            profile.Id = user.Id;
            await _userProfileInfoCollection.InsertOneAsync(profile);
        }
        #endregion
        /// <summary>
        /// Create instanse of saver.
        /// </summary>
        /// <param name="userResCollection">User resources collection to save in.</param>
        /// <param name="userProfCollection">User profile collection to save in.</param>
        public Saver(IMongoCollection<UserResources> userResCollection, IMongoCollection<UserInfo> userProfCollection)
        {
            _userResourcesCollection = userResCollection;
            _userProfileInfoCollection = userProfCollection;
        }
        /// <summary>
        /// Save change to user resource.
        /// </summary>
        public async void Save(IUser user, params ISavable<UserResources>[] savable)
        {
            if (savable.Length <= 0) throw new ArgumentNullException("Needed to use at least one saver argument");

            UserResources resources = await GetCurrentResources(user);
            Parallel.ForEach(savable, saver => saver.Save(ref resources));

            await SetCurrentResources(resources);
        }
        /// <summary>
        /// Save change to user profile.
        /// </summary>
        public async void Save(IUser user, params ISavable<UserInfo>[] savable)
        {
            if (savable.Length <= 0) throw new ArgumentNullException("Needed to use at least one saver argument");

            UserInfo profile = await GetCurrentProfile(user);
            foreach (var saver in savable)
            {
                saver.Save(ref profile);
            }

            await SetCurrentProfile(profile, user);
        }
    }
}
