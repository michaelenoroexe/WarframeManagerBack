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
        static UserRepository _userRepository;

        public UserController ()
        {
            _userRepository = new UserRepository();
        }

        // POST api/<RegistrationController>
        [HttpPost("api/registration")]
        public async Task<ActionResult> Post([FromBody] User user)
        {
            if (!await _userRepository.DataValidationAsync(user.Login)) return BadRequest("Invalid Login");
            if (!await _userRepository.DataValidationAsync(user.Password)) return BadRequest("Invalid Password");
            //Adding user to DB or error
            if (_userRepository.UserCheck(user.Login))
            {
                
                //await _userRepository._userCollection.InsertOneAsync(user);
                return Ok();
            }

            return Conflict(user.Login);           
        }
    }
}
