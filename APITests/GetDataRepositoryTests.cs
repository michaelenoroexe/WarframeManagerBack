using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.Repositories;
using API.Models;
using API.Models.Interfaces;
using MongoDB.Driver;
using NUnit.Framework;
using System.Linq.Expressions;
using MongoDB.Bson;
using NSubstitute;

namespace APITests
{
    public class GetDataRepositoryTests
    {
        [Test]
        public async Task GetResourcesList_ReturnResourcesList()
        {
            var getDataRepository = new FakeGetDataRepo(true);

            var res = await getDataRepository.GetResourcesListAsync();
            int resNum = res.Count;

            Assert.AreEqual(2, resNum);
        }
    }
    // Fake DataRepository class that break dependancies
    public class FakeGetDataRepo : GetDataRepository
    {
        public FakeGetDataRepo(bool test = false) : base(test)
        {
        }
        //public override Task<IAsyncCursor<T>> ReturnFindDataAsyncTask<T>(IMongoCollection<T>? collection)
        //{
        //    return null;
        //}

        //public override Task<List<R>> ReturnToListTaskFromAsyncFindTask<T, R>(T task)
        //{
        //    return Task.FromResult(new List<R>() {
        //        new R() { Id = ObjectId.GenerateNewId(), Name = "Salvage", Type = new string[]{"Resource"} },
        //        new R() { Id = ObjectId.GenerateNewId(), Name = "Galium", Type = new string[]{"Resource"} },
        //    });
        //}
    }
}
