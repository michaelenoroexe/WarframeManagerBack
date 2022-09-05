using API.Logger;
using API.Models;
using API.Models.Interfaces;
using API.Models.Responses;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
namespace API.Repositories
{
    public class GetDataRepository
    {
        private IMongoCollection<Item> _itemCollection;
        public readonly IMongoCollection<UserResources> _usersItemsCollection;
        public readonly IMongoCollection<Planet> _planets;
        private readonly ILogger _logger = new LoggerProvider(Path.Combine(Directory.GetCurrentDirectory(), "logger.txt")).CreateLogger("");

        public GetDataRepository(bool test = false)
        {
            if (!test)
            {
                _itemCollection = DBClient.db.GetCollection<Item>("Components");
                _usersItemsCollection = DBClient.db.GetCollection<UserResources>("UsersResources");
                _planets = DBClient.db.GetCollection<Planet>("Planets");
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
        public async Task<GetDataResponses> GetUserItAsync(Func<Task<List<Item>>> allIt, Func<ObjectId, Task<Dictionary<string, int>>> userIt, User user)
        {
            if (user == null) return new GetDataResponses(401);
            //Return full list of resources
            var allRessTask = allIt();
            var items = new List<Item>();

            var userRess = await userIt(user.Id);
            items = await allRessTask;
            List<Item> it = new List<Item>();
            Item? i;
            if (userRess != null)
                foreach (KeyValuePair<string, int> res in userRess)
                {
                    i = items.FirstOrDefault(re => re.strId == res.Key);
                    if (i == null) _logger.LogError($"User id:{user.Id} has incorrect item '{res.Key}:{res.Value}' in collection UserResources");
                    else
                    {
                        i.Owned = res.Value;
                        it.Append(i);
                    }
                }
            return new GetDataResponses(20, items);
        }
    }
}
