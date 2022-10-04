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

        // POST api/ProfUp
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
        // POST api/ProfUp/creds
        [HttpPost("creds")]
        public async Task<ActionResult> CredCh([FromBody] Cred num)
        {
            try
            {
                User user = await JwtAuthentication.GetUserFromTokenAsync(HttpContext.User.Claims.FirstOrDefault().Value);
                if (user == null) return Unauthorized();
                if (repository.UpdateCredits(user, num.Number) == false) return BadRequest();
                return Accepted();
            }
            catch
            {
                return BadRequest();
            }  
        }
        // POST api/ProfUp/userInfo
        [HttpPost("userInfo")]
        public async Task<ActionResult> ProfCh([FromBody] UserInfo ch)
        {
            try
            {
                User user = await JwtAuthentication.GetUserFromTokenAsync(HttpContext.User.Claims.FirstOrDefault().Value);
                if (user == null) return Unauthorized();
                if (repository.UpdateProfInfo(user, ch) == false) return BadRequest();
                return Accepted();
            }
            catch
            {
                return BadRequest();
            }
        }

        public class Change
        {
            public string Resource { get; set; }
            public int Number { get; set; }
            public string Type { get; set; }
        }

        public class Cred
        {
            public int Number { get; set; }
        }
    }
}
