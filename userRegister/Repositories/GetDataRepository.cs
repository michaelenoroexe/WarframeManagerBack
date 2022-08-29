using API.Models;
using API.Models.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;
namespace API.Repositories
{
    public class GetDataRepository
    {
        private IMongoCollection<Component> _compCollection;

        public GetDataRepository(bool test = false)
        {
            if (!test)
            {
                _compCollection = DBClient.db.GetCollection<Component>("Components");
            }                          
        }

        public async Task<List<Resource>> GetResourcesListAsync()
        {
            // Find data in DB
            return await Conv.ToResourceList(await _compCollection.FindAsync<Component>(FilterDefinition<Component>.Empty));      
        }

        public async Task<List<Component>> GetComponentsListAsync()
        {
            // Find data in DB
            return await _compCollection.FindAsync<Component>(x => x.Credits > 0).Result.ToListAsync();
        }
    }
}
