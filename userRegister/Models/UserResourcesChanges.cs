using API.Logger;
using API.Repositories;
using MongoDB.Bson;
using MongoDB.Driver;

namespace API.Models
{
    public class UserResourcesChanges : UserResources, IDisposable
    {
        private void CSetter<T>(ref T field, T val)
        {
            LastAcces = DateTime.Now;
            field = val;
        }

        // All changes in user item list, in buffer
        private Dictionary<string, int> _items = new Dictionary<string, int>();        
        new public Dictionary<string, int> Items { get { return _items; } set { CSetter(ref _items, value); } }
        // All changes in user resource list, in buffer
        private Dictionary<string, int> _resources = new Dictionary<string, int>();
        new public Dictionary<string, int> Resources{ get {return _resources;} set {CSetter(ref _resources, value); } }
        // Changes in user credits
        private int _credits = 0;
        new public int Credits { get { return _credits; } set { CSetter(ref _credits, value); } }
        // Changes in user profile info
        private UserInfo? _profInfo = null;
        public UserInfo? ProfInfo { get { return _profInfo; } set { CSetter(ref _profInfo, value); } }
        // Variables to make class auto time exposeble
        public DateTime LastAcces { get; set; }

        public Task DelayBeforeSavingToDB { get; set; }

        private int delayBeforeSave = (int)TimeSpan.FromMinutes(1).TotalMilliseconds;
        // User resources collection
        private IMongoCollection<UserResources> _usResCollection;
        // User page info collection
        private IMongoCollection<UserInfo> _usInfCollection;

        private readonly ILogger _logger = new LoggerProvider(Path.Combine(Directory.GetCurrentDirectory(), "logger.txt")).CreateLogger("");

        // Constructor with db Collection to sav e changes
        public UserResourcesChanges(in IMongoCollection<UserResources> usResCollection, in IMongoCollection<UserInfo> usInfCollection, ObjectId userId)
        {
            _usResCollection = usResCollection;
            _usInfCollection = usInfCollection;
            User = userId;
            LastAcces = DateTime.Now;
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
            if (ProfInfo is not null)
            {
                var userInfMass = await _usInfCollection.FindAsync(Builders<UserInfo>.Filter.Eq(x => x.Id, User));
                UserInfo userInf = await userInfMass.SingleOrDefaultAsync();
                if (userInf is null)
                {
                    userInf = ProfInfo;
                    await _usInfCollection.InsertOneAsync(userInf);                   
                }
                else
                {
                    await _usInfCollection.UpdateOneAsync(Builders<UserInfo>.Filter.Eq(x => x.Id, userInf.Id),
                                                                                   Builders<UserInfo>.Update.Set(x => x.Login, ProfInfo.Login)
                                                                                                            .Set(x => x.Rank, ProfInfo.Rank)
                                                                                                            .Set(x => x.Image, ProfInfo.Image));
                }
            }
            
            var userRessMass = await _usResCollection.FindAsync(Builders<UserResources>.Filter.Eq(x => x.User, User));
            UserResources userRess = await userRessMass.SingleOrDefaultAsync();
            // Generate new record if user still not in UserResources collection
            if (userRess is null)
            {
                userRess = new UserResources() { Id = ObjectId.GenerateNewId(), User = User, Items = Items, Resources = Resources, Credits = Credits };
                await _usResCollection.InsertOneAsync(userRess);
                return;
            }
            //Save user credits change
            if (Credits >= 0 && Credits != userRess?.Credits) 
                await _usResCollection.UpdateOneAsync(Builders<UserResources>.Filter.Eq(x => x.Id, userRess.Id),
                                                               Builders<UserResources>.Update.Set(x => x.Credits, Credits));
            if (Resources.Count > 0 && Items.Count > 0)
            {
                FillList(Resources, "resource",ref userRess);
                FillList(Items, "item", ref userRess);
                await _usResCollection.UpdateOneAsync(Builders<UserResources>.Filter.Eq(x => x.Id, userRess.Id),
                                                               Builders<UserResources>.Update.Set(x => x.Items, userRess.Items).Set(x => x.Resources, userRess.Resources));
                return;
            }
            if (Items.Count > 0)
            {
                FillList(Items, "item", ref userRess);
                await _usResCollection.UpdateOneAsync(Builders<UserResources>.Filter.Eq(x => x.Id, userRess.Id),
                                                               Builders<UserResources>.Update.Set(x => x.Items, userRess.Items));
                return;
            }
            if (Resources.Count > 0)
            {
                FillList(Resources, "resource", ref userRess);
                await _usResCollection.UpdateOneAsync(Builders<UserResources>.Filter.Eq(x => x.Id, userRess.Id),
                                                               Builders<UserResources>.Update.Set(x => x.Resources, userRess.Resources));
                return;
            }

        }

        private void FillList(Dictionary<string, int> change, string type, ref UserResources target)
        {
            if (type == "item")
            {
                if (target.Items == null) target.Items = new Dictionary<string, int>();
                foreach (KeyValuePair<string, int> ch in change)
                {
                    target.Items[ch.Key] = ch.Value;
                }
                return;
            }
            if (type == "resource")
            {
                if (target.Resources == null) target.Resources = new Dictionary<string, int>();
                foreach (KeyValuePair<string, int> ch in change)
                {
                    target.Resources[ch.Key] = ch.Value;
                }
                return;
            }
        }


        public void Dispose()
        {
        }
    }
}
