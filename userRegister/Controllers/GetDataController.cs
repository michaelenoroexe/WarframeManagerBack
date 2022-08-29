using Microsoft.AspNetCore.Mvc;
using API.Repositories;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/GetData")]
    [ApiController]
    public class GetDataController : ControllerBase
    {
        // GET: api/GetData/ResourcesList
        [HttpGet("ResourcesList")]
        public async Task<ActionResult> GetResourcesList()
        {
            //Return full list of resources
            var ar = new GetDataRepository();
            return Ok(await ar.GetResourcesListAsync());
        }

        [HttpGet("ComponentsList")]
        public async Task<ActionResult> GetComponentsList()
        {
            var ar = new GetDataRepository();
            return Ok(await ar.GetComponentsListAsync());
        }

        // GET api/<GetDataController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }
       
    }
}
