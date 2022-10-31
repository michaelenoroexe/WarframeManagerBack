using Microsoft.AspNetCore.Mvc;
using System.Collections.Specialized;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Web;
using API.Models;
using API.Repositories;
using API.Controllers;
using API;
using Microsoft.AspNetCore.Authorization;
using UserValidation;
using Shared;
using Shared;
using System.Security.Claims;
using API.Models.UserWork;

namespace API.Controllers
{
    // Control users requests about accounts 
    [Route("api")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserManager _userRepository;
        private IUserValidator<(string, string)> _passValidator;
        private IUserValidator<ClaimsPrincipal> _jwtValidator;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserManager userRepository, IUserValidator<(string, string)> passValidator, 
            IUserValidator<ClaimsPrincipal> jwtValidator, ILogger<UserController> logger)
        {
            _userRepository = userRepository;
            _passValidator = passValidator;
            _jwtValidator = jwtValidator;
            _logger = logger;
        }

        // Controller that process user registration requests
        [HttpPost("registration")]
        public async Task<ActionResult> UserRegister([FromBody] FullUser user)
        {
            //Checking user input on data validation
            try
            {
                _passValidator.ValidateUser(user);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
            //Adding user to DB or error
            await _userRepository.AddUserAsync(user, user.Password);
                
            return Ok();
        }
        // Controller that process user login in requests
        [HttpPost("signin")]
        public ActionResult UserSignIn([FromBody] ClientUser us)
        {
            IClientUser clientUser;
            IUser? user;
            //Checking user input on data validation
            try
            {
                if (us.Login is null) return BadRequest("Invalid Login");
                if (us.Password is null) return BadRequest("Invalid Password");
                clientUser = _passValidator.GetConverter().CreateUser(new (us.Login, us.Password));
                user = _passValidator.ValidateUser(clientUser);
                if (user == null) return BadRequest("User with this login is not created");
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
            return Ok(new {Token = _userRepository.LoginUser(user)});
        }

        // Controller that process user password change
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("passChange")]
        public async Task<ActionResult> UserPassChange([FromBody] (string OldPassword, string NewPassword) pasCh)
        {
            IUser? user;
            IClientUser clientUser;
            IUser? initialUser;
            try
            {
                // Validate user.
                clientUser = _jwtValidator.GetConverter().CreateUser(User);
                var us = new FullUser(ObjectId.Empty, clientUser.Login, pasCh.OldPassword);
                user = _passValidator.ValidateUser(us);
                if (user is null) return Unauthorized();
                initialUser = user;
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex.Message);
                // If old password is invalid
                if (ex.Message == "Password is incorrect.") return BadRequest("Invalid Old Password");
                if (ex.Message == "Password is not match.") return BadRequest("Wrong old Password");
                return BadRequest(ex.Message);
            }
            clientUser = new FullUser(user.Id, user.Login, pasCh.NewPassword);
            // Check if new password is invalid.
            try
            {
                user = _passValidator.ValidateUser(clientUser);
            }
            catch (ArgumentException ex)
            {
                if (ex.Message != "Password is not match.")
                    return BadRequest("Invalid New Password");
            }

            await _userRepository.ChangeUserPasswordAsync(initialUser, pasCh.NewPassword);

            return Accepted();
        }
        // Delete user account
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("delUser")]
        public async Task<ActionResult> UserDelete([FromBody] ReceivingPassword pas)
        {
            IClientUser clientUser;
            IUser? user;
            //Checking user input on data validation
            try
            {
                clientUser = _jwtValidator.GetConverter().CreateUser(User);
                clientUser = _passValidator.GetConverter().CreateUser((clientUser.Login, pas.Password));
                user = _passValidator.ValidateUser(clientUser);
                if (user == null) return Unauthorized();
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
            //Adding user to DB or error
            await _userRepository.DeleteUserAsync(user);

            return Accepted();
        }
    }
}
