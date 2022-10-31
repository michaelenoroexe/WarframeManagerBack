using MongoDB.Driver;

namespace UserValidation.DBSearcher
{
    internal sealed class DBUserSearcher
    {
        private IMongoCollection<FullUser> _userCollection;

        public DBUserSearcher(IMongoCollection<FullUser> userCollection)
            => _userCollection = userCollection;   

        public async Task<FullUser?> TryFindUserAsync(IClientUser clientUser)
        {
            Predicate<FullUser> filter = (x) => x.Login.Equals(clientUser.Login, StringComparison.OrdinalIgnoreCase);
            IAsyncCursor<FullUser> usersInDB = await _userCollection.FindAsync(x => filter(x));
            return usersInDB.SingleOrDefault();
        }
    }
}
