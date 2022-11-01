using MongoDB.Driver;
using Shared;

namespace API.Models.UserWork.Getter
{
    internal sealed class UserDBGetter : DBGetter
    {
        /// <summary>
        /// Get full user item list.
        /// </summary>
        private async Task<UserResources?> GetUserResources(IUser user)
        {
            IAsyncCursor<UserResources> userRes = await _resourceCollection.FindAsync(Builders<UserResources>.Filter.Eq(db => db.User, user.Id));
            return userRes.SingleOrDefault();
        }
        public UserDBGetter(IMongoCollection<UserResources> resourceCollection, IMongoCollection<UserInfo> profileCollection)
            : base(resourceCollection, profileCollection) { }

        public override async Task<int?> GetCredits(IUser user) => (await GetUserResources(user))?.Credits;
        public override async Task<Dictionary<string, int>?> GetItems(IUser user) => (await GetUserResources(user))?.Items;
        public override async Task<Dictionary<string, int>?> GetResources(IUser user) => (await GetUserResources(user))?.Resources;
        public override async Task<UserInfo?> GetProfile(IUser user)
        {
            IAsyncCursor<UserInfo> userRes = await _profileCollection.FindAsync(Builders<UserInfo>.Filter.Eq(db => db.Id, user.Id));
            return userRes.SingleOrDefault();
        }
    }
}
