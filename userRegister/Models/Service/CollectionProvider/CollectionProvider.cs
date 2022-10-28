using API.Models.Common;
using API.Models.Interfaces;
using MongoDB.Driver;

namespace API.Models.Service
{
    sealed class CollectionProvider : ICollectionProvider
    {
        private readonly Dictionary<IResource, int> _items;
        private readonly Dictionary<IResource, int> _resources;

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
            return res.ToEnumerable().Where(func);
        }
        /// <summary>
        /// Fill dictionary with items fromm collection as key and 0 as value.
        /// </summary>
        private void FillDictionary(Dictionary<IResource, int> dictionary, IEnumerable<IResource> collection)
        {
            foreach (IResource resource in collection)
            {
                dictionary.Add(resource, 0);
            }
        }
        /// <summary>
        /// Create collection provider with full list of items.
        /// </summary>
        /// <param name="fullList">Full list of items from DB.</param>
        public CollectionProvider(IMongoCollection<Item> fullList)
        {
            _items = new Dictionary<IResource, int>();
            _resources = new Dictionary<IResource, int>();
            var getCollectionByPredicate = (Func<Item, bool> arg) => GetFullList(fullList, arg);

            Task<IEnumerable<IResource>> resourcesGetTask = getCollectionByPredicate(item => item.IsResource());
            Task<IEnumerable<IResource>> itemsGetTask = getCollectionByPredicate(item => !item.IsResource());

            Task fillResources = resourcesGetTask.ContinueWith(res => FillDictionary(_resources, res.Result));
            Task fillItems = itemsGetTask.ContinueWith(res => FillDictionary(_items, res.Result));
            fillResources.Start();
            fillItems.Start();

            Task.WaitAll(fillResources, fillItems);
        }
        /// <summary>
        /// Get all items with own number 0.
        /// </summary>
        /// <returns></returns>
        public Dictionary<IResource, int> GetAllItems() => _items;
        /// <summary>
        /// Get all resources with own number 0.
        /// </summary>
        public Dictionary<IResource, int> GetAllResources() => _resources;
    }
}
