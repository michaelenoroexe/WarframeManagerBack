using API.Models.Common;
using API.Models.Common.ItemComp;
using API.Models.Interfaces;
using API.Models.UserWork.Interfaces;
using MongoDB.Driver;
using Shared;
using System.Collections;
using System.Runtime.ExceptionServices;

namespace API.Repositories
{
    internal sealed class GetDataRepository : IGetData
    {
        private readonly ICollectionProvider _provider;
        private readonly IUserInfoGetter _userInfoGetter;
        private readonly IMongoCollection<Planet> _planets;
        private readonly IMongoCollection<Restype> _types;
        private readonly ILogger _logger;

        /// <summary>
        /// Fill total item list with users owned items.
        /// </summary>
        /// <param name="dict">Key value collection of user item numer.</param>
        /// <param name="items">Total item list.</param>
        /// <exception cref="ArgumentNullException">If one of item is invalid.</exception>
        /// <exception cref="ArgumentException">If key of dictionary not in item list.</exception>
        private void FillItemNumber(in Dictionary<string, int> dict, ref IResource[] items)
        {
            if (dict is null) throw new ArgumentNullException("Dictionary with items number is null");
            if (items is null) throw new ArgumentNullException("Dont have collection to search in");

            IResource? i;
            int index;
            ExceptionDispatchInfo? exep = null;
            foreach (KeyValuePair<string, int> res in dict)
            {
                index = ((IList)items).IndexOf(res.Key);
                if (index != -1) i = items[index].Clone() as IResource;
                else i = null;

                if (i is null)
                {
                    if (exep is null) exep = ExceptionDispatchInfo.Capture(new ArgumentException());
                    exep!.SourceException.Data.Add(res.Key, res.Value);
                }
                else
                {
                    i.SetOwned(res.Value);
                    items[index] = i;
                }
            }
            // Throw if user have incorrect items.
            exep?.Throw();
        }
        /// <summary>
        /// Decorator for fillItemNumber with error hangdling.
        /// </summary>
        /// <param name="dict">Dictionary with users items.</param>
        /// <param name="itemList">Full list of items.</param>
        /// <param name="user">user resources are filled to log if exeption.</param>
        private void FillWithCatch(in Dictionary<string, int> dict, ref IResource[] itemList, IUser user)
        {
            try
            {
                FillItemNumber(in dict, ref itemList);
            }
            catch (ArgumentException ex)
            {
                foreach (DictionaryEntry e in ex.Data)
                {
                    _logger.LogError($"User id:{user.Id} has incorrect item '{e.Key}:{e.Value}' in buffer");
                }
            }
        }

        public GetDataRepository(IMongoCollection<Planet> planetColl, IMongoCollection<Restype> resColl, ICollectionProvider collectionProvider,
            IUserInfoGetter userInfoGetter, ILogger<GetDataRepository> logger)
        {
            _provider = collectionProvider;
            _userInfoGetter = userInfoGetter;
            _planets = planetColl;
            _types = resColl;
            _logger = logger;
        }

        public IResource[] GetItemsList() => _provider.GetAllItems();
        public IResource[] GetResourcesList() => _provider.GetAllResources();
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
        public async Task<List<Restype>> GetTypesListAsync() => await _types.FindAsync(FilterDefinition<Restype>.Empty).Result.ToListAsync();
        public async Task<int> GetUserCreditsAsync(IUser user) => await _userInfoGetter.GetCreditsAsync(user);
        public async Task<UserInfo> GetUserInfoAsync(IUser user) => await _userInfoGetter.GetProfileAsync(user);
        public async Task<IResource[]> GetUserItemsAsync(IUser user)
        {
            IResource[] fullList = GetItemsList().ToArray();
            Dictionary<string, int>? userItems = await _userInfoGetter.GetFullItemAsync(user);

            if (userItems is null) return fullList;

            FillWithCatch(in userItems, ref fullList, user);

            return fullList;
        }
        public async Task<IResource[]> GetUserResourcesAsync(IUser user)
        {
            IResource[] fullList = GetResourcesList();
            Dictionary<string, int>? userResources = await _userInfoGetter.GetFullResourceAsync(user);

            if (userResources is null) return fullList;

            FillWithCatch(in userResources, ref fullList, user);

            return fullList;
        }
    }
}
