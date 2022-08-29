using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using API.Repositories;
using API.Models;
using MongoDB.Bson;

namespace APITests
{
    public class ProfileUpdateRepositoryTests
    {
        [Test]
        public void GetAllUsersResourcesShouldReturnAllRecords()
        {
            var rep = new ProfileUpdateRepository();

            var res = rep.GetAllUsersResources();

            Assert.AreNotEqual(res, null);
        }

        [Test]
        public async Task UpdateUserResourcesShouldReturnTrue()
        {
            var user = new User() { Id = ObjectId.Parse("6267fea599251c426804d69f"), Login = "arrr", Password = "ssssss" };
            var item = new KeyValuePair<string, int>("6305344e9070664c88e5fa28", new Random().Next(1000));
            var rep = new ProfileUpdateRepository();

            var res = await rep.UpdateUserResourcesAsync(user, item);
            Assert.IsTrue(res);
        }
    }
}
