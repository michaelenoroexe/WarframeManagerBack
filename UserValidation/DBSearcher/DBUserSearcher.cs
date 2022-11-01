using MongoDB.Driver;

namespace UserValidation.DBSearcher
{
    /// <summary>
    /// Search user in DB.
    /// </summary>
    internal sealed class DBUserSearcher
    {
        private readonly IMongoCollection<FullUser> _userCollection;

        public DBUserSearcher(IMongoCollection<FullUser> userCollection)
            => _userCollection = userCollection;
        /// <summary>
        /// Try find user in database by login.
        /// </summary>
        /// <param name="clientUser">User info.</param>
        /// <returns>Return full user from datavase if user with inputed login exists, otherwise null.</returns>
        public async Task<FullUser?> TryFindUserAsync(IClientUser clientUser)
        {
            IAsyncCursor<FullUser> usersInDB = await _userCollection.FindAsync
            (
                x => x.Login.Equals(clientUser.Login)
            );
            return usersInDB.SingleOrDefault();
        }
    }
}
