using API.Models;
using API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/ProfUp")]
    [ApiController]
    public class ProfileUpdateController : ControllerBase
    {
        private ProfileUpdateRepository repository = new ProfileUpdateRepository();
        // GET: api/<ProfileUpdateController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<ProfileUpdateController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ProfileUpdateController>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Change res)
        {
            User user = await JwtAuthentication.GetUserFromTokenAsync(HttpContext.User.Claims.FirstOrDefault().Value);
            if (user == null) return Unauthorized();
            if (res.Type == "resource")
                if (await repository.UpdateUserResourcesAsync(user, new KeyValuePair<string, int>(res.Resource, res.Number))) return Accepted();
            if (res.Type == "item")
                if (await repository.UpdateUserItemsAsync(user, new KeyValuePair<string, int>(res.Resource, res.Number))) return Accepted();
            return BadRequest();
        }

        // PUT api/<ProfileUpdateController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ProfileUpdateController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        public class Change
        {
            public string Resource { get; set; }
            public int Number { get; set; }
            public string Type { get; set; }
        }
    }
}
