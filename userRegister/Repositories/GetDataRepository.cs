using API.Models;
using API.Models.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;
namespace API.Repositories
{
    public class GetDataRepository
    {
        private IMongoCollection<Component> _compCollection;
        public readonly IMongoCollection<UserResources> _usersItemsCollection;

        public GetDataRepository(bool test = false)
        {
            if (!test)
            {
                _compCollection = DBClient.db.GetCollection<Component>("Components");
                _usersItemsCollection = DBClient.db.GetCollection<UserResources>("UsersResources");
            }                          
        }

        public async Task<List<Component>> GetResourcesListAsync()
        {
            // Find data in DB
            return await _compCollection.FindAsync<Component>(FilterDefinition<Component>.Empty).Result.ToListAsync();      
        }

        public async Task<List<Component>> GetComponentsListAsync()
        {
            // Find data in DB
            return await _compCollection.FindAsync<Component>(x => x.Credits > 0).Result.ToListAsync();
        }

        public async Task<Dictionary<string, int>> GetUsersItemsAsync(ObjectId userId)
        {
            var ress = await _usersItemsCollection.FindAsync(Builders<UserResources>.Filter.Eq(db => db.User, userId));
            return ress.SingleOrDefault().Items;
        }
    }
}
