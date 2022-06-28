using API.Models;
using API.Models.Interfaces;
using MongoDB.Driver;
namespace API.Repositories
{
    public class GetDataRepository
    {
        private IMongoCollection<Resource> _resCollection;
        public IMongoCollection<Resource> ResCollection { get { return _resCollection; } set { _resCollection = value; } }
        private IMongoCollection<Component> _compCollection;
        public IMongoCollection<Component> CompCollection { get { return _compCollection; } set { _compCollection = value; } }

        public GetDataRepository(bool test = false)
        {
            if (!test)
            {
                _resCollection = DBClient.db.GetCollection<Resource>("Resources");
                _compCollection = DBClient.db.GetCollection<Component>("Components");
            }                          
        }

        public async Task<List<Resource>> GetResourcesListAsync()
        {
            // Find data in DB
            var AllResFind = ReturnFindDataAsyncTask(_resCollection);
            var AllCompsFind = ReturnFindDataAsyncTask(_compCollection);
            // Wait data and then conver to list
            var AllRes = ReturnToListTaskFromAsyncFindTask<Task<IAsyncCursor<Resource>>, Resource>(AllResFind);
            var AllComps = ReturnToListTaskFromAsyncFindTask<Task<IAsyncCursor<Component>>, Component>(AllCompsFind);
            // Wait converion
            await AllRes;
            await AllComps;
            return AllRes.Result.Concat(AllComps.Result).ToList();          
        }
        // Create and return task that find data in DB
        public virtual Task<IAsyncCursor<T>> ReturnFindDataAsyncTask<T>(IMongoCollection<T> collection)
        {
            return collection.FindAsync(FilterDefinition<T>.Empty);
        }
        // Await data from finding task and create task that conver to list
        public virtual Task<List<R>> ReturnToListTaskFromAsyncFindTask<T, R> (T task) where T : Task<IAsyncCursor<R>> where R: IResource, new()
        {    
            return task.WaitAsync(TimeSpan.FromSeconds(10)).Result.ToListAsync<R>();
        }
    }
}
