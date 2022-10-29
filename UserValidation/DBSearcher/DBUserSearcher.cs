﻿using MongoDB.Driver;

namespace UserValidation.DBSearcher
{
    internal class DBUserSearcher
    {
        private IMongoCollection<FullUser> _userCollection;

        public DBUserSearcher(IMongoCollection<FullUser> userCollection)
        {
            _userCollection = userCollection;
        }
        
        public async Task<FullUser?> TryFindUserAsync(IClientUser clientUser)
        {
            IAsyncCursor<FullUser> usersInDB = await _userCollection.FindAsync( x => x.Login == clientUser.Login);
            return usersInDB.SingleOrDefault();
        }
    }
}
