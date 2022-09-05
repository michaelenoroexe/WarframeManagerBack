using Microsoft.AspNetCore.Mvc;
using API.Repositories;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using MongoDB.Bson;
using API.Models.Responses;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/GetData")]
    [ApiController]
    public class GetDataController : ControllerBase
    {
        private GetDataRepository repository = new();
        private readonly ILogger<GetDataController> _logger;

        public GetDataController(ILogger<GetDataController> logger)
        {
            _logger = logger;
        }
        // GET: api/GetData/ResourcesList
        [HttpGet("UserResourcesList")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult> GetUserResourcesList()
        {
            var user = await JwtAuthentication.GetUserFromTokenAsync(HttpContext.User.Claims.FirstOrDefault().Value);
            GetDataResponses res = await repository.GetUserItAsync(repository.GetResourcesListAsync, repository.GetUsersResourcesAsync, user);
            if (res.Code == 20) return Ok(res.Data);
            return BadRequest(res.Data);
        }

        [HttpGet("ResourcesList")]
        public async Task<ActionResult> GetResourcesList()
        {
            //Return full list of resources
            return Ok(await repository.GetResourcesListAsync());
        }

        [HttpGet("UserItemsList")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult> GetUserItemsList()
        {
            var user = await JwtAuthentication.GetUserFromTokenAsync(HttpContext.User.Claims.FirstOrDefault().Value);
            GetDataResponses res = await repository.GetUserItAsync(repository.GetItemsListAsync, repository.GetUsersItemsAsync, user);
            if (res.Code == 20) return Ok(res.Data);
            return BadRequest(res.Data);
        }

        [HttpGet("ComponentsList")]
        public async Task<ActionResult> GetItemsList()
        {
            // Return full list of components
            return Ok(await repository.GetItemsListAsync());
        }

        [HttpGet("Planets")]
        public async Task<ActionResult> GetPlanets()
        {
            return Ok(await repository.GetPlanetListAsync());
        }

        // GET api/<GetDataController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }
       
    }
}
