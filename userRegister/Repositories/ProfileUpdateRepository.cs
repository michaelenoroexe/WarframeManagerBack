using API.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace API.Repositories
{
    public sealed class ProfileUpdateRepository
    {
        private readonly IMongoCollection<UserResources> _usersItemsCollection;

        private readonly IMongoCollection<UserInfo> _usersInfCollection;

        public ProfileUpdateRepository()
        {
            _usersItemsCollection = DBClient.Db.GetCollection<UserResources>("UsersResources");
            _usersInfCollection = DBClient.Db.GetCollection<UserInfo>("UsersInfo");
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
                    changes = new UserResourcesChanges(in _usersItemsCollection, in _usersInfCollection, user.Id);
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
                    changes = new UserResourcesChanges(in _usersItemsCollection, in _usersInfCollection, user.Id);
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

        public bool UpdateCredits(User user, int num)
        {
            try
            {
                var changes = UserResourcesChangesBuffer._totalBuffer.FirstOrDefault(userChan => userChan.User == user.Id);
                if (changes == null)
                {
                    changes = new UserResourcesChanges(in _usersItemsCollection, in _usersInfCollection, user.Id);

                    UserResourcesChangesBuffer._totalBuffer.Add(changes);

                }
                changes.Credits = num;
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool UpdateProfInfo(User user, UserInfo inf)
        {
            try
            {
                var changes = UserResourcesChangesBuffer._totalBuffer.FirstOrDefault(userChan => userChan.User == user.Id);
                if (changes == null)
                {
                    changes = new UserResourcesChanges(in _usersItemsCollection, in _usersInfCollection, user.Id);

                    UserResourcesChangesBuffer._totalBuffer.Add(changes);

                }
                changes.ProfInfo = new UserInfo(user, inf.Rank, inf.Image);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
