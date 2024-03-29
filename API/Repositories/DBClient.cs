﻿using MongoDB.Driver;

namespace API.Repositories
{
    /// <summary>
    /// Client with access to DB.
    /// </summary>
    internal sealed class DBClient
    {
        private readonly IMongoClient _mongoClient;
        /// <summary>
        /// Working database
        /// </summary>
        public IMongoDatabase Db { get; }
        public DBClient(string mongoUrl)
        {
            _mongoClient = new MongoClient(mongoUrl);
            Db = _mongoClient.GetDatabase("WarframeManager");
        }
    }
}
