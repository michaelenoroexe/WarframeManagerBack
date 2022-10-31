using API.Models.Common.ItemComp;

namespace API.Models.Interfaces
{
    internal interface ICollectionProvider
    {
        public IEnumerable<IResource> GetAllResources();

        public IEnumerable<IResource> GetAllItems();
    }
}
