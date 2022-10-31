using API.Models.Common.ItemComp;

namespace API.Models.Interfaces
{
    internal interface ICollectionProvider
    {
        public IResource[] GetAllResources();

        public IResource[] GetAllItems();
    }
}
