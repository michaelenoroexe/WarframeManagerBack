using API.Models.UserWork.Interfaces;
using BufferUserRequests;
using Shared;

namespace API.Models.UserWork.Getter
{
    public class UserBufferChecker : IBufferChecker
    {
        private UsersChangeBuffer _changesBuffer;
        /// <summary>
        /// Check curent values in buffer of user changes.
        /// </summary>
        /// <param name="changesBuffer"></param>
        public UserBufferChecker(UsersChangeBuffer changesBuffer)
        {
            _changesBuffer = changesBuffer;
        }
        public int? GetCredits(IUser user) => _changesBuffer.TryGetUserChanges(user)?.GetCurrentCreditNumber();
        public Dictionary<string, int>? GetItems(IUser user) => _changesBuffer.TryGetUserChanges(user)?.GetCurrentItemList();
        public UserInfo? GetProfile(IUser user) => _changesBuffer.TryGetUserChanges(user)?.GetCurrentProfileInfo();
        public Dictionary<string, int>? GetResources(IUser user) => _changesBuffer.TryGetUserChanges(user)?.GetCurrentResourceList();
    }
}
