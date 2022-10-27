using API.Models;
using API.Models.Interfaces;
using API.Models.Responses;
using API.Models.Service;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections;
using System.Runtime.ExceptionServices;

namespace API.Repositories
{
    public sealed class GetDataRepository : IGetData
    {
        private readonly IMongoCollection<Item> _itemCollection;
        private readonly IMongoCollection<UserResources> _usersItemsCollection;
        private readonly IMongoCollection<UserInfo> _usersInfoCollection;
        private readonly IMongoCollection<Planet> _planets;
        private readonly IMongoCollection<Restype> _types;
        private readonly ILogger _logger;

        /// <summary>
        /// Return full list of items filtered by predicate.
        /// </summary>
        /// <param name="func">Predicate to filter resulte list</param>
        /// <returns></returns>
        private async Task<List<Item>> GetFullList(Func<Item, bool> func)
        {
            // Get all items from db.
            IAsyncCursor<Item> res = await _itemCollection.FindAsync<Item>(FilterDefinition<Item>.Empty);
            // Filter items py predicate.
            return res.ToEnumerable().Where(func).ToList();
        } 
        /// <summary>
        /// Get full users items list.
        /// </summary>
        private async Task<UserResources?> GetUserFullList(ObjectId userId)
        {
            IAsyncCursor<UserResources> ress = await _usersItemsCollection.FindAsync(Builders<UserResources>.Filter.Eq(db => db.User, userId));
            return ress.SingleOrDefault();
        }
        /// <summary>
        /// Fill total item list with users owned items.
        /// </summary>
        /// <param name="dict">Key value collection of user item numer.</param>
        /// <param name="items">Total item list.</param>
        /// <exception cref="ArgumentNullException">If one of item is invalid.</exception>
        /// <exception cref="ArgumentException">If key of dictionary not in item list.</exception>
        private void FillItemNumber(in IEnumerable<KeyValuePair<string, int>> dict, in List<Item> items)
        {
            if (dict is null) throw new ArgumentNullException("Dictionary with items number is null");
            if (items is null) throw new ArgumentNullException("Dont have collection to search in");

            Item? i;
            ExceptionDispatchInfo? exep = null;
            foreach (KeyValuePair<string, int> res in dict)
            {
                i = items.FirstOrDefault(re => re.strId.Equals(res.Key, StringComparison.OrdinalIgnoreCase));

                if (i is null)
                {
                    if (exep is null) exep = ExceptionDispatchInfo.Capture(new ArgumentException());
                    exep!.SourceException.Data.Add(res.Key, res.Value);
                }
                else i.Owned = res.Value;
            }
            // Throw if user have incorrect items.
            exep?.Throw();
        }

        public GetDataRepository(ILogger<GetDataRepository> logger)
        {
                _itemCollection = DBClient.Db.GetCollection<Item>("Components");
                _usersItemsCollection = DBClient.Db.GetCollection<UserResources>("UsersResources");
                _usersInfoCollection = DBClient.Db.GetCollection<UserInfo>("UsersInfo");
                _planets = DBClient.Db.GetCollection<Planet>("Planets");
                _types = DBClient.Db.GetCollection<Restype>("Types");
                _logger = logger;
        }
        /// <summary>
        /// Get all resource list.
        /// </summary>
        public async Task<List<Item>> GetResourcesListAsync() => await GetFullList(db => Resource.IsResource(db));
        /// <summary>
        /// Get all item list.
        /// </summary>
        /// <returns></returns>
        public async Task<List<Item>> GetItemsListAsync() => await GetFullList(db => !Resource.IsResource(db));
        /// <summary>
        /// Get all user resources.
        /// </summary>
        /// <param name="userId">UserId which resources needed to get.</param>
        /// <returns>Dictionary of resource name as key, and resource number as value.  Null if user dont have any.</returns>
        public async Task<Dictionary<string, int>?> GetUsersResourcesAsync(ObjectId userId)
            => (await GetUserFullList(userId))?.Resources;
        /// <summary>
        /// Get all user items.
        /// </summary>
        /// <param name="userId">UserId which items needed to get.</param>
        /// <returns>Dictionary of item name as key, and item number as value. Null if user dont have any.</returns>
        public async Task<Dictionary<string, int>?> GetUsersItemsAsync(ObjectId userId)
            => (await GetUserFullList(userId))?.Items;
        /// <summary>
        /// Get dictionary with all planets.
        /// </summary>
        /// <returns>All planets id/name.</returns>
        public async Task<Dictionary<string, string>> GetPlanetListAsync()
        {
            var planets = new Dictionary<string, string>();

            IAsyncCursor<Planet> res = await _planets.FindAsync(FilterDefinition<Planet>.Empty);            
            await res.ForEachAsync(plan => planets.Add(plan.Id.ToString(), plan.Name));

            return planets;
        }

        // Decide what user get items all resources.
        public async Task<List<Item>> GetUserItAsync(Func<Task<List<Item>>> allIt, Func<ObjectId, Task<Dictionary<string, int>>> userIt, User user, Dictionary<string, int>? bufres = null)
        {
            //Return full list of resources.
            var allRessTask = allIt();
            List<Item> items;

            Dictionary<string, int> userRess = await userIt(user.Id);          
            items = await allRessTask;

            IEnumerable<KeyValuePair<string, int>>? userRessNotInBuffer = null;
            if (bufres is not null)
            {
                IEqualityComparer<KeyValuePair<string, int>> equalityComparer = new KeyValEqualityComparer<string, int>();

                userRessNotInBuffer = userRess.Except(bufres, equalityComparer);
                try
                {
                    FillItemNumber(bufres, items);
                }
                catch (ArgumentNullException ex)
                {
                    _logger.LogCritical("Null argument in setting user buffer items:" + ex.Message);
                    throw;
                }
                catch (ArgumentException ex)
                {
                    foreach (DictionaryEntry e in ex.Data)
                    {
                        _logger.LogError($"User id:{user.Id} has incorrect item '{e.Key}:{e.Value}' in buffer");
                    }
                }
            }

            try
            { 
                FillItemNumber(userRessNotInBuffer ?? userRess, items);
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogCritical("Null argument in setting user items:" + ex.Message);
                throw;
            }
            catch (ArgumentException ex)
            {
                foreach (DictionaryEntry e in ex.Data)
                {
                    _logger.LogError($"User id:{user.Id} has incorrect item '{e.Key}:{e.Value}' in db");
                }
            }
                           
            return items;
        }

        public async Task<List<Restype>> GetTypesListAsync()
        {       
            return await _types.FindAsync(FilterDefinition<Restype>.Empty).Result.ToListAsync();
        }

        public async Task<int> GetUserCredits(IUser user)
        {
            try
            {
                var ress = await _usersItemsCollection.FindAsync(Builders<UserResources>.Filter.Eq(db => db.User, user.Id));
                UserResources res = await ress.SingleOrDefaultAsync();
                if (res == null || res?.Credits <= 0) 
                {
                    return 0;

                }
                return res.Credits;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<UserInfo> GetUserInfo(User user)
        {
            try
            {
                var ress = await _usersInfoCollection.FindAsync(Builders<UserInfo>.Filter.Eq(db => db.Login, user.Login));
                UserInfo res = await ress.SingleOrDefaultAsync();
                
                if (res is null) return new UserInfo(user, 0, 0);

                return res;
            }
            catch (Exception ex)
            {
                return new UserInfo(user, 0, 0);
            }
        }
    }
}
