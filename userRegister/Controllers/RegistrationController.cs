using Microsoft.AspNetCore.Mvc;

namespace userRegister.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        // GET: api/<RegistrationController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            var val = new string[] { "value1", "value2" };
            
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
        public void Post([FromBody] string login, string password)
        {
            Console.WriteLine(login);
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
