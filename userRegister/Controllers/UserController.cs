using Microsoft.AspNetCore.Mvc;
using System.Collections.Specialized;
using MongoDB.Driver;
using MongoDB.Bson;
//JSON
using System.Web;
using API.Models;
using API.Repositories;
using API.Controllers;
using API;
namespace API.Controllers
{

    [Route("")]
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
        public async Task<ActionResult> UserRegister([FromBody] User user)
        {
            try
            {
                //Checking user input on data validation
                if (!_userRepository.DataValidation(user.Login)) throw new Exception("Invalid Login");
                if (!_userRepository.DataValidation(user.Password)) throw new Exception("Invalid Password");
                //Adding user to DB or error
                var ans = await _userRepository.AddUserAsync(user);
                
                if (ans.Success) return Ok(ans);
                throw new Exception(ans.ErrorMessage);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Invalid Login" 
                    || ex.Message == "Invalid Password") return BadRequest(ex.Message);
                if (ex.Message == "A user with the given login already exists.")
                    return Conflict(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("api/signin")]
        public async Task<ActionResult> UserSignIn([FromBody] User user)
        {
            try
            {
                //Checking user input on data validation
                if (!_userRepository.DataValidation(user.Login)) throw new Exception("Invalid Login");
                if (!_userRepository.DataValidation(user.Password)) throw new Exception("Invalid Password");
                //Adding user to DB or error
                var ans = await _userRepository.LoginUserAsync(user);

                if (ans.Success) return Ok(ans);
                throw new Exception(ans.ErrorMessage);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Invalid Login"
                    || ex.Message == "Invalid Password") return BadRequest(ex.Message);
                if (ex.Message == "A user with the given login already exists.")
                    return Conflict(ex.Message);
                return StatusCode(500, "Server error");
            }
        }
    }
}
