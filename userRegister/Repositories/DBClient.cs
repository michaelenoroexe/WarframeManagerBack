using MongoDB.Driver;

namespace API.Repositories
{
    public static class DBClient
    {
        private static MongoClient MongoClient { get; }
        public static IMongoDatabase db { get; set; }

        static DBClient()
        {
            MongoClient = new MongoClient("mongodb+srv://warframe_manager_user:H9guvYhcVtWk5z25@warframemanagercluster.jvusw.mongodb.net/WarframeManager?retryWrites=true&w=majority");
            db = MongoClient.GetDatabase("WarframeManager");
        }
    }
}
