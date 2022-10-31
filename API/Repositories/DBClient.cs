using MongoDB.Driver;

namespace API.Repositories
{
    // Static client to connect to DB
    internal sealed class DBClient
    {
        private MongoClient _mongoClient { get; }
        /// <summary>
        /// Working database
        /// </summary>
        public IMongoDatabase Db { get; }
        public DBClient(string mongoUrl)
        {
            _mongoClient = new MongoClient(mongoUrl);
            Db = _mongoClient.GetDatabase("WarframeManager");
        }
    }
}
