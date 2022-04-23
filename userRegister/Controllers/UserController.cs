using Microsoft.AspNetCore.Mvc;
using System.Collections.Specialized;
using Newtonsoft.Json;
using MongoDB.Driver;
using MongoDB.Bson;
//JSON
using System.Web;
using API.Models;
using API.Repositories;
using API.Controllers;

namespace API.Controllers
{

//    [Route("api/registration")]
    [ApiController]
    public class UserController : ControllerBase
    {
        static IMongoDatabase db = DBClient.db;


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
        [HttpPost("api/registration")]
        public async Task<ActionResult> Post([FromBody] User user)
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
