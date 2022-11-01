using API.Models.Common.ItemComp;

namespace API.Models.Service
{
    /// <summary>
    /// Provider of static caсhed information from DB. 
    /// </summary>
    internal interface ICollectionProvider
    {
        /// <summary>
        /// Get full list of resources.
        /// </summary>
        /// <returns>List of resources presented as IResource array.</returns>
        public IResource[] GetAllResources();
        /// <summary>
        /// Get full list of items.
        /// </summary>
        /// <returns>List of items presented as IResource array.</returns>
        public IResource[] GetAllItems();
    }
}
