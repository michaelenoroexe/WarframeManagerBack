using Microsoft.AspNetCore.Mvc;
using System.Collections.Specialized;
using Newtonsoft.Json;
using MongoDB.Driver;
using MongoDB.Bson;
//JSON
using System.Web;

namespace userRegister.Controllers
{
    public class User
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        static MongoClient client = new MongoClient("mongodb+srv://warframe_manager_user:H9guvYhcVtWk5z25@warframemanagercluster.jvusw.mongodb.net/WarframeManager?retryWrites=true&w=majority");
        static IMongoDatabase db = client.GetDatabase("WarframeManager");

        //Checking database for existing usename
        static bool UserCheck(IMongoCollection<BsonDocument> col, string us)
        {
            BsonDocument userlogin = new Dictionary<string, string>() { { "Login", us } }.ToBsonDocument();
            //userlogin.AddRange(new Dictionary<string, string>({ "Login", us }));            
            List<BsonDocument> ans = col.Find(userlogin).Limit(1).ToList();
            if (ans.Count == 0) return true;
            if (ans.Count == 1) return false;
            if (ans.Count > 1 | ans.Count < 0) throw new IndexOutOfRangeException();
            return false;
        }

        // POST api/<RegistrationController>
        [HttpPost]
        public ActionResult Post([FromBody] User user)
        {
            var us = user.ToBsonDocument();
            var collection = db.GetCollection<BsonDocument>("Users");
            //Adding user to DB or error
            if (UserCheck(collection, user.Login))
            {
//                collection.InsertOne(us);
                return Ok();
            } else
            {
                return Conflict(user.Login);
            }            
        }
    }
}
