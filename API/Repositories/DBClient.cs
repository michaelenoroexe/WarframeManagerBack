using MongoDB.Driver;

namespace API.Repositories
{
    // Static client to connect to DB
    public class DBClient
    {
        private static readonly DBClient _instance;
        private MongoClient MongoClient { get; }
        private DBClient()
        {
            // Chosing path depend on environment. 
            string? MongoURL = Environment.GetEnvironmentVariable("MongoClientUrl");

            if (MongoURL is null) MongoURL = "mongodb+srv://warframe_manager_user:H9guvYhcVtWk5z25@warframemanagercluster.jvusw.mongodb.net/WarframeManager?retryWrites=true&w=majority";

            MongoClient = new MongoClient(MongoURL);
            Db = MongoClient.GetDatabase("WarframeManager");
        }
        /// <summary>
        /// Get instance of DBClient.
        /// </summary>
        public static DBClient GetDBClient() => _instance;
        /// <summary>
        /// Working database
        /// </summary>
        public IMongoDatabase Db { get; }

        static DBClient()
        {
            _instance = new DBClient();
        }
    }
}
