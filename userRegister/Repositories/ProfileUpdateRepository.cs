using API.Models;
using API.Models.Interfaces;
using API.Models.UserWork;
using API.Models.UserWork.Setter;
using MongoDB.Bson;
using MongoDB.Driver;
using Shared;

namespace API.Repositories
{
    internal sealed class ProfileUpdateRepository : IChangeData
    {
        private readonly ICollectionProvider _provider;
        private readonly IBufferChanger _userInfoSetter;
        private readonly ILogger _logger;

        public ProfileUpdateRepository(ICollectionProvider collectionProvider, IBufferChanger userSetter, ILogger<GetDataRepository> logger)
        {
            _provider = collectionProvider;
            _userInfoSetter = userSetter;
            _logger = logger;
        }

        public void UpdateCredits(IUser user, int credits) => _userInfoSetter.SetCreditNumber(user, credits);

        public void UpdateItem(IUser user, KeyValuePair<string, int> item) => _userInfoSetter.SetItemNum(user, item);

        public void UpdateProfile(IUser user, UserInfo userInfo) => _userInfoSetter.SetProfileInfo(user, userInfo);

        public void UpdateResource(IUser user, KeyValuePair<string, int> res) => _userInfoSetter.SetResourcesNum(user, res);
    }
}
