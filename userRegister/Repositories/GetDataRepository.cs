using API.Logger;
using API.Models;
using API.Models.Interfaces;
using API.Models.Responses;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
namespace API.Repositories
{
    public sealed class GetDataRepository
    {
        private readonly IMongoCollection<Item> _itemCollection;
        private readonly IMongoCollection<UserResources> _usersItemsCollection;
        private readonly IMongoCollection<UserInfo> _usersInfoCollection;
        private readonly IMongoCollection<Planet> _planets;
        private readonly IMongoCollection<Restype> _types;
        private readonly ILogger _logger = new LoggerProvider(Path.Combine(Directory.GetCurrentDirectory(), "logger.txt")).CreateLogger("");

        public GetDataRepository(bool test = false)
        {
            if (!test)
            {
                _itemCollection = DBClient.Db.GetCollection<Item>("Components");
                _usersItemsCollection = DBClient.Db.GetCollection<UserResources>("UsersResources");
                _usersInfoCollection = DBClient.Db.GetCollection<UserInfo>("UsersInfo");
                _planets = DBClient.Db.GetCollection<Planet>("Planets");
                _types = DBClient.Db.GetCollection<Restype>("Types");
            }
        }

        public async Task<List<Item>> GetResourcesListAsync()
        {
            // Find data in DB
            return _itemCollection.FindAsync<Item>(FilterDefinition<Item>.Empty).Result.ToListAsync().Result.Where(db => Resource.IsResource(db)).ToList();
        }

        public async Task<List<Item>> GetItemsListAsync()
        {
            // Find data in DB
            return _itemCollection.FindAsync<Item>(FilterDefinition<Item>.Empty).Result.ToListAsync().Result.Where(db => !Resource.IsResource(db)).ToList();
        }

        public async Task<Dictionary<string, int>> GetUsersResourcesAsync(ObjectId userId)
        {
            var ress = await _usersItemsCollection.FindAsync(Builders<UserResources>.Filter.Eq(db => db.User, userId));
            return ress.SingleOrDefault().Resources;
        }

        public async Task<Dictionary<string, int>> GetUsersItemsAsync(ObjectId userId)
        {
            var ress = await _usersItemsCollection.FindAsync(Builders<UserResources>.Filter.Eq(db => db.User, userId));
            return ress.SingleOrDefault().Items;
        }

        public async Task<Dictionary<string, string>> GetPlanetListAsync()
        {
            var res = new Dictionary<string, string>();
            _planets.FindAsync(FilterDefinition<Planet>.Empty).Result.ToListAsync().Result.ForEach(x => res.Add(x.Id.ToString(), x.Name));
            return res;
        }

        // Decide what user get items all resources
        public async Task<GetDataResponses> GetUserItAsync(Func<Task<List<Item>>> allIt, Func<ObjectId, Task<Dictionary<string, int>>> userIt, User user, Dictionary<string, int>? bufres = null)
        {
            if (user == null) return new GetDataResponses(401);
            //Return full list of resources
            var allRessTask = allIt();
            var items = new List<Item>();

            var userRess = await userIt(user.Id);
            items = await allRessTask;
            //List<Item> it = new List<Item>();
            Item? i;
            if (userRess is not null)
                foreach (KeyValuePair<string, int> res in userRess)
                {
                    i = items.FirstOrDefault(re => re.strId == res.Key);
                    if (i == null) _logger.LogError($"User id:{user.Id} has incorrect item '{res.Key}:{res.Value}' in collection UserResources");
                    else
                    {
                        i.Owned = res.Value;
                        //it.Append(i);
                    }
                }
            if (bufres is not null)
                foreach (KeyValuePair<string, int> res in bufres)
                {
                    i = items.FirstOrDefault(re => re.strId == res.Key);
                    if (i == null) _logger.LogError($"User id:{user.Id} has incorrect item '{res.Key}:{res.Value}' in changes UserResources");
                    else
                    {
                        i.Owned = res.Value;
                        //it.Append(i);
                    }
                }
                
            return new GetDataResponses(20, items);
        }

        public async Task<List<Restype>> GetTypesListAsync()
        {       
            return await _types.FindAsync(FilterDefinition<Restype>.Empty).Result.ToListAsync();
        }

        public async Task<int> GetUserCredits(User user)
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
                if (res == null)
                {
                    return new UserInfo(user, 0, 0);

                }
                return res;
            }
            catch (Exception ex)
            {
                return new UserInfo(user, 0, 0);
            }
        }
    }
}
