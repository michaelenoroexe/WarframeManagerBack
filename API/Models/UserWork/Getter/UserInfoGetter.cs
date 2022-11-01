using API.Models.UserWork.Interfaces;
using Shared;

namespace API.Models.UserWork.Getter
{
    internal sealed class UserInfoGetter : IUserInfoGetter
    {
        private readonly IBufferChecker _bufferChecker;
        private readonly DBGetter _dbGetter;
        /// <summary>
        /// Add aditional dictionary values to main.
        /// </summary>
        private static Dictionary<string, int>? DictionaryFiller(Dictionary<string, int>? main, Dictionary<string, int>? addit)
        {
            if (addit is null && main is null) return null;
            if (addit is null) return main;
            if (main is null) return addit;
            foreach (KeyValuePair<string, int> item in addit)
                main[item.Key] = item.Value;
            return main;
        }

        /// <summary>
        /// Instantiate UserInfoGetter.
        /// </summary>
        /// <param name="bufferChecker">Buffer with user resources checker.</param>
        /// <param name="dBGetter">Database info getter.</param>
        public UserInfoGetter(IBufferChecker bufferChecker, DBGetter dBGetter)
        {
            _bufferChecker = bufferChecker;
            _dbGetter = dBGetter;
        }
        /// <summary>
        /// Get user credits numner, 0 if user dont have credits.
        /// </summary>
        public async Task<int> GetCreditsAsync(IUser user)
            => _bufferChecker.GetCredits(user) ?? await _dbGetter.GetCredits(user) ?? 0;
        /// <summary>
        /// Get user profile info, new default profile if user dont have record.
        /// </summary>
        public async Task<UserInfo> GetProfileAsync(IUser user)
            => _bufferChecker.GetProfile(user) ?? await _dbGetter.GetProfile(user) ?? new UserInfo(user, 0, 0);
        /// <summary>
        /// Get user items and number, null if user dont have record in db and in buffer.
        /// </summary>
        public async Task<Dictionary<string, int>?> GetFullItemAsync(IUser user)
            => DictionaryFiller(await _dbGetter.GetItems(user), _bufferChecker.GetItems(user));
        /// <summary>
        /// Get user resource and number, null if user dont have record in db and in buffer.
        /// </summary>
        public async Task<Dictionary<string, int>?> GetFullResourceAsync(IUser user)
            => DictionaryFiller(await _dbGetter.GetResources(user), _bufferChecker.GetResources(user));
    }
}
