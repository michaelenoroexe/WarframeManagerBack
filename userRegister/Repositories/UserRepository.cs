﻿using API.Models;
using API.Models.Responses;
using Microsoft.Extensions.Options;
using API.Repositories;
using MongoDB.Driver;
using MongoDB.Bson;
using API.Logger;

namespace API.Repositories
{
    public sealed class UserRepository
    {
        private readonly IMongoCollection<User> _userCollection;
        private readonly ILogger _logger = new LoggerProvider(Path.Combine(Directory.GetCurrentDirectory(), "logger.txt")).CreateLogger("");
        public UserRepository(bool test = false)
        {
            if (!test)
            {
                _userCollection = DBClient.Db.GetCollection<User>("Users");
            }                                
        }
        // Finding single user, when 0 = null, if users more than one = exeption.
        public async Task<User?> FindUserAsync(string us)
        {
            try
            {
                return await _userCollection
                    .Find(new BsonDocument("Login", us))
                    .SingleOrDefaultAsync();
            }
            catch (MongoException ex)
            {
                _logger.LogCritical(ex.Message);
                return null;
            }
        }
        // Async function that valid and add user into database.
        public async Task<UserResponse> AddUserAsync(User us)
        {
            try
            {
                User? user = await FindUserAsync(us.Login);
                if (user != null) throw new Exception("Already in database");
                user = new User() { Login = us.Login, Password = Hash.HashString(us.Password)};
                await _userCollection.WithWriteConcern(new WriteConcern(1)).InsertOneAsync(user);
                return new UserResponse(user);
                throw new Exception("Unknown Error");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                if (ex.Message == "Already in database")
                    return new UserResponse(false, "A user with the given login already exists.");
                 return new UserResponse(false, ex.Message);
            }
        }
        // Async function that valid and sign user in.
        public async Task<UserResponse> LoginUserAsync(User us)
        {
            try
            {
                User? user = await FindUserAsync(us.Login);
                if (user == null || !Hash.Verify(us.Password, user.Password)) throw new Exception("Wrong Login or Password!");

                return new UserResponse(true, JwtAuthentication.GenerateToken(user));
                throw new Exception("Unknown Error");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                if (ex.Message == "Already in database")
                    return new UserResponse(false, "A user with the given login already exists.");
                return new UserResponse(false, ex.Message);
            }
        }
        // Async function that valid and sign user in.
        public async Task<UserResponse> ChangeUserPasswordAsync(User us, string newPassword)
        {
            try
            {
                await _userCollection.UpdateOneAsync(Builders<User>.Filter.Eq(x => x.Id, us.Id),
                                               Builders<User>.Update.Set(db => db.Password, Hash.HashString(newPassword)));
                return new UserResponse(true, "Accepted");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new UserResponse(false, ex.Message);
            }
        }
        // Async function that delete user from db.
        public async Task<UserResponse> DeleteUserAsync(User us)
        {
            try
            {
                var usDel = _userCollection.FindOneAndDeleteAsync(Builders<User>.Filter.Eq(x => x.Id, us.Id));
                var usInfDel = DBClient.Db.GetCollection<UserInfo>("UsersInfo").FindOneAndDeleteAsync(Builders<UserInfo>.Filter.Eq(x => x.Id, us.Id));
                var usResDel = DBClient.Db.GetCollection<UserResources>("UsersResources").FindOneAndDeleteAsync(Builders<UserResources>.Filter.Eq(x => x.User, us.Id));
                UserResourcesChangesBuffer._totalBuffer.Nodes().FirstOrDefault(x => x.ValueRef.User == us.Id)?.ValueRef.Dispose();
                await usDel;
                await usInfDel;
                await usResDel;
                return new UserResponse(true, "Accepted");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new UserResponse(false, ex.Message);
            }
        }
    }
}
