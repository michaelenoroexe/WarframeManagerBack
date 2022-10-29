using BufferUserRequests.ChangeManagers;
using Shared;

namespace BufferUserRequests
{
    public interface IManagersStorage
    {
        public ISavableChangeManager<KeyValuePair<string, int>, Dictionary<string, int>, UserResources> GetResourceManager();
        public ISavableChangeManager<KeyValuePair<string, int>, Dictionary<string, int>, UserResources> GetItemsManager();
        public ISavableChangeManager<int, UserResources> GetCreditManager();
        public ISavableChangeManager<UserInfo, UserInfo> GetProfileManager();
        public ISavableChangeManager<KeyValuePair<string, int>, Dictionary<string, int>, UserResources>? TryGetResourceManager();
        public ISavableChangeManager<KeyValuePair<string, int>, Dictionary<string, int>, UserResources>? TryGetItemsManager();
        public ISavableChangeManager<int, UserResources>? TryGetCreditManager();
        public ISavableChangeManager<UserInfo, UserInfo>? TryGetProfileManager();
        public void Save();
    }
}
