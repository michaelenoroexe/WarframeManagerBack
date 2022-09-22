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
            var changes = UserResourcesChangesBuffer._totalBuffer.FirstOrDefault(userChan => userChan.User == user.Id);
            GetDataResponses res = await repository.GetUserItAsync(repository.GetResourcesListAsync, repository.GetUsersResourcesAsync, user, changes?.Resources);
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
            var changes = UserResourcesChangesBuffer._totalBuffer.FirstOrDefault(userChan => userChan.User == user.Id);
            GetDataResponses res = await repository.GetUserItAsync(repository.GetItemsListAsync, repository.GetUsersItemsAsync, user, changes?.Items);
            if (res.Code == 20) return Ok(res.Data);
            return BadRequest(res.Data);
        }

        [HttpGet("ItemsList")]
        public async Task<ActionResult> GetItemsList()
        {
            // Return full list of components
            return Ok(await repository.GetItemsListAsync());
        }

        [HttpGet("TypesList")]
        public async Task<ActionResult> GetTypesList()
        {
            // Return full list of components
            return Ok(await repository.GetTypesListAsync());
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
        [HttpGet("UserCredits")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult> GetUserCredits()
        {
            var user = await JwtAuthentication.GetUserFromTokenAsync(HttpContext.User.Claims.FirstOrDefault().Value);
            Task<int> res = repository.GetUserCredits(user);
            var changes = UserResourcesChangesBuffer._totalBuffer.FirstOrDefault(userChan => userChan.User == user.Id);
            if (changes?.Credits != null && changes.Credits != 0) return Ok(changes.Credits);
            
             return Ok(await res);
        }
    }
}
