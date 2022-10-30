using API.Models.Common;
using API.Models.Common.ItemComp;
using API.Models.Interfaces;
using MongoDB.Driver;

namespace API.Models.Service
{
    sealed class CollectionProvider : ICollectionProvider
    {
        private IEnumerable<IResource> _items;
        private IEnumerable<IResource> _resources;

        /// <summary>
        /// Return full list of items filtered by predicate.
        /// </summary>
        /// <param name="func">Predicate to filter resulte list.</param>
        /// <returns></returns>
        private async Task<IEnumerable<IResource>> GetFullList(IMongoCollection<Item> collection, Func<Item, bool> func)
        {
            // Get all items from db.
            IAsyncCursor<Item> res = await collection.FindAsync<Item>(FilterDefinition<Item>.Empty);
            // Filter items py predicate.
            return res.ToEnumerable().Where(func).ToList();
        }
        /// <summary>
        /// Create collection provider with full list of items.
        /// </summary>
        /// <param name="fullList">Full list of items from DB.</param>
        public CollectionProvider(IMongoCollection<Item> fullList)
        {
            var getCollectionByPredicate = (Func<Item, bool> arg) => GetFullList(fullList, arg);

            Task<IEnumerable<IResource>> resourcesGetTask = getCollectionByPredicate(item => item.IsResource());
            Task<IEnumerable<IResource>> itemsGetTask = getCollectionByPredicate(item => !item.IsResource());

            _resources = resourcesGetTask.Result;
            _items = itemsGetTask.Result;
        }
        /// <summary>
        /// Get all items with own number 0.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IResource> GetAllItems() => _items;
        /// <summary>
        /// Get all resources with own number 0.
        /// </summary>
        public IEnumerable<IResource> GetAllResources() => _resources;
    }
}
