using Microsoft.AspNetCore.Mvc;
using API.Repositories;
using API.Models;
using Microsoft.AspNetCore.Authorization;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/GetData")]
    [ApiController]
    public class GetDataController : ControllerBase
    {
        // GET: api/GetData/ResourcesList
        [HttpGet("UserResourcesList")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult> GetUserResourcesList()
        {
            var user = await JwtAuthentication.GetUserFromTokenAsync(HttpContext.User.Claims.FirstOrDefault().Value);
            //Return full list of resources
            var rep = new GetDataRepository();
            var allRessTask = rep.GetResourcesListAsync();
            var ress = new List<Component>();
            if (user != null)
            {
                var userRess = await rep.GetUsersItemsAsync(user.Id);
                ress = await allRessTask;
                foreach (KeyValuePair<string, int> res in userRess)
                {
                    ress.First(re => re.strId == res.Key).Owned = res.Value;
                }
                return Ok(ress);
            }
            return Unauthorized();
        }
        [HttpGet("ResourcesList")]
        public async Task<ActionResult> GetResourcesList()
        {
            var user = await JwtAuthentication.GetUserFromTokenAsync(HttpContext.User.Claims.FirstOrDefault().Value);
            //Return full list of resources
            var rep = new GetDataRepository();
            return Ok(await rep.GetResourcesListAsync());
        }
        [HttpGet("ComponentsList")]
        public async Task<ActionResult> GetComponentsList()
        {
            var rep = new GetDataRepository();
            return Ok(await rep.GetComponentsListAsync());
        }

        [HttpGet]

        // GET api/<GetDataController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }
       
    }
}
