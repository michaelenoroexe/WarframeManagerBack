namespace API.Models.Interfaces
{
    internal interface ICollectionProvider
    {
        public Dictionary<IResource, int> GetAllResources();

        public Dictionary<IResource, int> GetAllItems();
    }
}
