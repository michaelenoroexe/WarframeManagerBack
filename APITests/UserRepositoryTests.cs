using System;
using System.Linq;
using API.Models;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using API.Repositories;

namespace APITests
{
    public class UserRepositoryTests
    {
        private UserRepository _userRepository;

        [SetUp]
        public void Setup()
        {
            _userRepository = new UserRepository(true);
        }
        // Test that method work normaly with valid data.
        [Test]
        public void DataValidationAsyncTest()
        {
            var checkData = _userRepository.DataValidation;
            // Valid data check
            string[] cdata = { @"dima", @"misha", @"vladimir"
                               ,"a<script>alert(1)</script>"
                               ,@"q!#$%&()*+,-./;<=>?@[\]^_{|}~" };
            foreach (string data in cdata)
            Assert.IsTrue(checkData(data), data);

            // Invalid data check
            string[] wdata = { @"hi", @"������", @"User incorrect"                           
                               ,@"1ih", @"/is\", @"/@happy@/", @""
                               ,@"hihihihihihihihihihihihihihihihihihihihihihihihihihihihihihihihihihihihi" };
            foreach (string data in wdata)
            Assert.IsTrue(!checkData(data), data);
        }

    }
}