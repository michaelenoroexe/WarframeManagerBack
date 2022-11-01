using API.Models.UserWork.Setter;
using Shared;

namespace API.Repositories
{
    /// <summary>
    /// Repository to process user data changing requests.
    /// </summary>
    internal sealed class ProfileUpdateRepository : IChangeData
    {
        private readonly IBufferChanger _userInfoSetter;
        private readonly ILogger _logger;

        public ProfileUpdateRepository(IBufferChanger userSetter, ILogger<GetDataRepository> logger)
        {
            _userInfoSetter = userSetter;
            _logger = logger;
        }
        public void UpdateCredits(IUser user, int credits) => _userInfoSetter.SetCreditNumber(user, credits);
        public void UpdateItem(IUser user, KeyValuePair<string, int> item) => _userInfoSetter.SetItemNum(user, item);
        public void UpdateProfile(IUser user, UserInfo userInfo) => _userInfoSetter.SetProfileInfo(user, userInfo);
        public void UpdateResource(IUser user, KeyValuePair<string, int> res) => _userInfoSetter.SetResourcesNum(user, res);
    }
}
