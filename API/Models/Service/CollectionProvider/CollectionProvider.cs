using API.Models.Common.ItemComp;
using MongoDB.Driver;

namespace API.Models.Service
{
    internal sealed class CollectionProvider : ICollectionProvider
    {
        private readonly IResource[] _items;
        private readonly IResource[] _resources;

        /// <summary>
        /// Return full list of items filtered by predicate.
        /// </summary>
        /// <param name="func">Predicate to filter resulte list.</param>
        private static async Task<IResource[]> GetFullList(IMongoCollection<Item> collection, Func<Item, bool> func)
        {
            // Get all items from db.
            IAsyncCursor<Item> res = await collection.FindAsync<Item>(FilterDefinition<Item>.Empty);
            // Filter items py predicate.
            return res.ToEnumerable().Where(func).ToArray();
        }
        /// <summary>
        /// Create collection provider with full list of items.
        /// </summary>
        /// <param name="fullList">Full list of items from DB.</param>
        public CollectionProvider(IMongoCollection<Item> fullList)
        {
            var getCollectionByPredicate = (Func<Item, bool> arg) => GetFullList(fullList, arg);

            Task<IResource[]> resourcesGetTask = getCollectionByPredicate(item => item.IsResource());
            Task<IResource[]> itemsGetTask = getCollectionByPredicate(item => !item.IsResource());

            _resources = resourcesGetTask.Result;
            _items = itemsGetTask.Result;
        }
        /// <summary>
        /// Get all items with own number 0.
        /// </summary>
        public IResource[] GetAllItems() => (IResource[])_items.Clone();
        /// <summary>
        /// Get all resources with own number 0.
        /// </summary>
        public IResource[] GetAllResources() => (IResource[])_resources.Clone();
    }
}
