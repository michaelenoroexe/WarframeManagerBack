using Microsoft.AspNetCore.Mvc;
using System.Collections.Specialized;
using Newtonsoft.Json;
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
        // GET: api/<RegistrationController>
        [HttpGet]
        public IEnumerable<string> Get()
        {            
            return new string[] { "value1", "value2" };
        }

        // GET api/<RegistrationController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<RegistrationController>
        [HttpPost]
        public string Post([FromBody] User user)
        {
//            string[] js = JsonConvert.DeserializeObject<JsonReader>(us);
//            string[] user = JsonReader(us);
            return user.Login;
        }

        // PUT api/<RegistrationController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<RegistrationController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
