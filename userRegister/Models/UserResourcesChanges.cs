using API.Logger;
using API.Repositories;
using MongoDB.Bson;
using MongoDB.Driver;

namespace API.Models
{
    public class UserResourcesChanges : UserResources, IDisposable
    {
        private Dictionary<string, int> _items = new Dictionary<string, int>();
        // Set new Last access when items is changed
        new public Dictionary<string, int> Items { 
            get { return _items; } 
            set 
            {
                LastAcces = DateTime.Now;
                _items = value;
            } 
        }
        // Variables to make class auto time exposeble
        public DateTime LastAcces { get; set; }

        public Task DelayBeforeSavingToDB { get; set; }

        private int delayBeforeSave = (int)TimeSpan.FromMinutes(1).TotalMilliseconds;

        private IMongoCollection<UserResources> _dbCollection;

        private readonly ILogger _logger = new LoggerProvider(Path.Combine(Directory.GetCurrentDirectory(), "logger.txt")).CreateLogger("");

        // Constructor with db Collection to sav e changes
        public UserResourcesChanges(IMongoCollection<UserResources> dbCollection, ObjectId userId, KeyValuePair<string, int> items)
        {
            _dbCollection = dbCollection;
            User = userId;
            LastAcces = DateTime.Now;
            Items[items.Key] = items.Value;
            SaveToDB();
        }

        public async void SaveToDB()
        {
            // Generate task that will save and dispose object after some time with no connect
            DelayBeforeSavingToDB = Task.Run(async () => {
                while ((int)(DateTime.Now - LastAcces).TotalMilliseconds < delayBeforeSave)
                {
                    await Task.Delay(delayBeforeSave - (int)(DateTime.Now - LastAcces).TotalMilliseconds);
                }
                save();
            });
        }
        public void save()
        {
            _logger.LogInformation($"Saving '{User}' changes to DB");
            FindAndSaveAsync();
            UserResourcesChangesBuffer._totalBuffer.Remove(this);
            this.Dispose();
        }

        private async void FindAndSaveAsync()
        {
            var userRessMass = await _dbCollection.FindAsync(Builders<UserResources>.Filter.Eq(x => x.User, User));
            UserResources userRess = await userRessMass.SingleOrDefaultAsync();
            if (userRess == null)
            {
                userRess = new UserResources() { Id = ObjectId.GenerateNewId(), User = User, Items = Items };
                await _dbCollection.InsertOneAsync(userRess);
                return;
            }
            foreach (KeyValuePair<string, int> ch in Items)
            {
                userRess.Items[ch.Key] = ch.Value;
            }
            await _dbCollection.UpdateOneAsync(Builders<UserResources>.Filter.Eq(x => x.Id, userRess.Id),
                                               Builders<UserResources>.Update.Set(x => x.Items, userRess.Items));
        }

        public void Dispose()
        {
        }
    }
}
