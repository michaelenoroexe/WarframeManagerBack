using MongoDB.Driver;

namespace API.Repositories
{
    // Static client to connect to DB
    public static class DBClient
    {
        private static MongoClient MongoClient { get; }
        public static IMongoDatabase Db { get; }

        static DBClient()
        {    
            // Chosing path depend on environment 
            string? MongoURL = Environment.GetEnvironmentVariable("MongoClientUrl");
            if (MongoURL == null ){
                MongoURL = "mongodb+srv://warframe_manager_user:H9guvYhcVtWk5z25@warframemanagercluster.jvusw.mongodb.net/WarframeManager?retryWrites=true&w=majority";
            }else{
                // string MongoUser = Environment.GetEnvironmentVariable("MONGO_USER");
                // string MongoPass = Environment.GetEnvironmentVariable("MONGO_PASS");
                MongoURL = Environment.GetEnvironmentVariable("MongoClientUrl");
            }
            MongoClient = new MongoClient(MongoURL);
            Db = MongoClient.GetDatabase("WarframeManager");
        }
    }
}
