using API.Models.Service;
using BufferUserRequests;
using MongoDB.Bson;
using MongoDB.Driver;
using Shared;
using UserValidation;

namespace API.Repositories
{
    /// <summary>
    /// Repository to process user administration requests.
    /// </summary>
    internal sealed class UserRepository : IUserManager
    {
        private readonly IMongoCollection<FullUser> _userCollection;
        private readonly IMongoCollection<UserInfo> _userProfileCollection;
        private readonly IMongoCollection<UserResources> _userResourceCollection;
        private readonly UsersChangeBuffer _userChangeBuffer;
        private readonly IPasswordHasher _hasher;
        private readonly ILogger _logger;

        public UserRepository(IMongoCollection<FullUser> userCollection, IMongoCollection<UserInfo> userProfileCollection,
            IMongoCollection<UserResources> userResourceCollection, IPasswordHasher hasher, UsersChangeBuffer usersChangeBuffer, ILogger<UserRepository> logger)
        {
            _userCollection = userCollection;
            _userProfileCollection = userProfileCollection;
            _userResourceCollection = userResourceCollection;
            _userChangeBuffer = usersChangeBuffer;
            _hasher = hasher;
            _logger = logger;
        }

        public async Task AddUserAsync(IUser user, string password)
        {
            var us = new FullUser(ObjectId.GenerateNewId(), user.Login, _hasher.HashString(password));
            await _userCollection.WithWriteConcern(new WriteConcern(1)).InsertOneAsync(us);
        }
        public string SignInUser(IUser user) => JwtAuthentication.GenerateToken(user);
        public async Task ChangeUserPasswordAsync(IUser user, string newPassword)
        {
            try
            {
                await _userCollection.UpdateOneAsync(Builders<FullUser>.Filter.Eq(x => x.Id, user.Id),
                                               Builders<FullUser>.Update.Set(db => db.Password, _hasher.HashString(newPassword)));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
        public async Task DeleteUserAsync(IUser user)
        {
            try
            {
                var usDel = _userCollection.FindOneAndDeleteAsync<FullUser>(Builders<FullUser>.Filter.Eq(x => x.Id, user.Id));
                var usInfDel = _userProfileCollection.FindOneAndDeleteAsync(Builders<UserInfo>.Filter.Eq(x => x.Id, user.Id));
                var usResDel = _userResourceCollection.FindOneAndDeleteAsync(Builders<UserResources>.Filter.Eq(x => x.User, user.Id));
                _userChangeBuffer.GetUserChanges(user).Dispose();
                await usDel;
                await usInfDel;
                await usResDel;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
    }
}
