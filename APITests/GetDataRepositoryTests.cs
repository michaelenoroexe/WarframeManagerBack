using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.Repositories;
using API.Models;
using MongoDB.Driver;
using NUnit.Framework;
using System.Linq.Expressions;
using MongoDB.Bson;

namespace APITests
{
    public class GetDataRepositoryTests
    {
        private GetDataRepository _getDataRepository;

        [Test]
        public async Task GetResourcesList_ReturnResourcesList()
        {
            var getDataRepository = new GetDataRepository();
            var res = await getDataRepository.GetResourcesListAsync();
            int resNum = res.Count;

            Assert.Greater(resNum, 0);
        }        
    }
}
