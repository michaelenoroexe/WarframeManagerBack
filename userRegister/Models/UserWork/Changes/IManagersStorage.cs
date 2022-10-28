using API.Models.Interfaces;

namespace API.Models.UserWork.Changes
{
    internal interface IManagersStorage
    {
        public ISavableChangeManager<KeyValuePair<IResource, int>, Dictionary<IResource, int>, UserResources> GetResourceManager();
        public ISavableChangeManager<KeyValuePair<IResource, int>, Dictionary<IResource, int>, UserResources> GetItemsManager();
        public ISavableChangeManager<int, UserResources> GetCreditManager();
        public ISavableChangeManager<UserInfo, UserInfo> GetProfileManager();
        public void Save();
    }
}
