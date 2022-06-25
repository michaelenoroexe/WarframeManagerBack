using API.Models;
using MongoDB.Driver;
namespace API.Repositories
{
    public class GetDataRepository
    {
        private IMongoCollection<Resource> _resCollection;
        private IMongoCollection<Component> _compCollection;

        public GetDataRepository()
        {
            _resCollection = DBClient.db.GetCollection<Resource>("Resources");
            _compCollection = DBClient.db.GetCollection<Component>("Components");          
        }

        public async Task<List<Resource>> GetResourcesListAsync()
        {

            List<Resource> totalList = new List<Resource>();

            var AllResFind =_resCollection.FindAsync(_ => true);
            var AllCompsFind = _compCollection.FindAsync(_ => true);
            var AllRes = AllResFind.WaitAsync(TimeSpan.FromSeconds(10)).Result.ToListAsync<Resource>();
            var AllComps = AllCompsFind.WaitAsync(TimeSpan.FromSeconds(10)).Result.ToListAsync<Component>();
            await AllRes;
            totalList = AllRes.Result;
            await AllComps;
            totalList.Concat(AllComps.Result);
            return totalList;          
        }
    }
}
