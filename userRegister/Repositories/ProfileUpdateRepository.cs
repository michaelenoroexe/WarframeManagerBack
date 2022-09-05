using API.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace API.Repositories
{
    public class ProfileUpdateRepository
    {
        public readonly IMongoCollection<UserResources> _usersItemsCollection;

        public ProfileUpdateRepository()
        {
            _usersItemsCollection = DBClient.db.GetCollection<UserResources>("UsersResources");
        }

        public List<UserResources> GetAllUsersResources()
        {
            return _usersItemsCollection.Find(FilterDefinition<UserResources>.Empty).ToList();
        }

        public async Task<bool> UpdateUserResourcesAsync(User user, KeyValuePair<string, int> res)
        {
            try
            {
                var changes = UserResourcesChangesBuffer._totalBuffer.FirstOrDefault(userChan => userChan.User == user.Id);
                if (changes == null)
                {
                    changes = new UserResourcesChanges(_usersItemsCollection, user.Id);
                    changes.Resources[res.Key] = res.Value;
                    UserResourcesChangesBuffer._totalBuffer.Add(changes);
                    return true;
                }
                changes.Resources[res.Key] = res.Value;
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }         
        }

        public async Task<bool> UpdateUserItemsAsync(User user, KeyValuePair<string, int> item)
        {
            try
            {
                var changes = UserResourcesChangesBuffer._totalBuffer.FirstOrDefault(userChan => userChan.User == user.Id);
                if (changes == null)
                {
                    changes = new UserResourcesChanges(_usersItemsCollection, user.Id);
                    changes.Items[item.Key] = item.Value;
                    UserResourcesChangesBuffer._totalBuffer.Add(changes);
                    return true;
                }
                changes.Items[item.Key] = item.Value;
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
