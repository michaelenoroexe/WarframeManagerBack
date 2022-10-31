using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace APITests
{
    public class UserRepositoryTests
    {
        //// Test that method work normaly with valid data.
        //[Test]
        //public void DataValidationAsyncTest()
        //{
        //    var checkData = _userRepository.DataValidation;
        //    // Valid data check
        //    string[] cdata = { @"dima", @"misha", @"vladimir"
        //                       ,"a<script>alert(1)</script>"
        //                       ,@"q!#$%&()*+,-./;<=>?@[\]^_{|}~" };
        //    foreach (string data in cdata)
        //    Assert.IsTrue(checkData(data), data);

        //    // Invalid data check
        //    string[] wdata = { @"hi", @"Привет", @"User incorrect"                           
        //                       ,@"1ih", @"/is\", @"/@happy@/", @""
        //                       ,@"hihihihihihihihihihihihihihihihihihihihihihihihihihihihihihihihihihihihi" };
        //    foreach (string data in wdata)
        //    Assert.IsTrue(!checkData(data), data);
        //}
    }
}