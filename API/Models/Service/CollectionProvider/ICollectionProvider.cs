using API.Models.Common.ItemComp;

namespace API.Models.Service
{
    internal interface ICollectionProvider
    {
        public IResource[] GetAllResources();

        public IResource[] GetAllItems();
    }
}
