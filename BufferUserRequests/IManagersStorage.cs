using Shared;

namespace BufferUserRequests
{
    public interface IManagersStorage
    {
        public ISavableChangeManager<int, UserResources>? TryGetCreditManager();
        public ISavableChangeManager<int, UserResources> GetCreditManager();
        public ISavableChangeManager<UserInfo, UserInfo>? TryGetProfileManager();
        public ISavableChangeManager<UserInfo, UserInfo> GetProfileManager();
        public ISavableChangeManager<KeyValuePair<string, int>, Dictionary<string, int>, UserResources> GetItemsManager();
        public ISavableChangeManager<KeyValuePair<string, int>, Dictionary<string, int>, UserResources>? TryGetItemsManager();
        public ISavableChangeManager<KeyValuePair<string, int>, Dictionary<string, int>, UserResources> GetResourceManager();
        public ISavableChangeManager<KeyValuePair<string, int>, Dictionary<string, int>, UserResources>? TryGetResourceManager();
        public void Save();
    }
}
