using BufferUserRequests;
using Shared;

namespace API.Models.UserWork.Setter
{
    public sealed class UserBufferChanger : IBufferChanger
    {
        private readonly UsersChangeBuffer _changesBuffer;
        /// <summary>
        /// Change values in buffer of user changes.
        /// </summary>
        /// <param name="changesBuffer"></param>
        public UserBufferChanger(UsersChangeBuffer changesBuffer) => _changesBuffer = changesBuffer;
        public void SetCreditNumber(IUser user, int credits) => _changesBuffer.GetUserChanges(user).ChangeCreditNumber(credits);
        public void SetProfileInfo(IUser user, UserInfo profile) => _changesBuffer.GetUserChanges(user).ChangeProfileInfo(profile);
        public void SetItemNum(IUser user, KeyValuePair<string, int> item) => _changesBuffer.GetUserChanges(user).ChangeItemList(item);
        public void SetResourcesNum(IUser user, KeyValuePair<string, int> resource) => _changesBuffer.GetUserChanges(user).ChangeResourceList(resource);
    }
}
