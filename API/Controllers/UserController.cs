using API.Models.UserWork;
using API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Shared;
using System.Security.Claims;
using UserValidation;

namespace API.Controllers
{
    // Control users requests about accounts 
    [Route("api")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserManager _userRepository;
        private readonly IUserValidator<(string, string)> _passValidator;
        private readonly IUserValidator<ClaimsPrincipal> _jwtValidator;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserManager userRepository, IUserValidator<(string, string)> passValidator,
                              IUserValidator<ClaimsPrincipal> jwtValidator, ILogger<UserController> logger)
        {
            _userRepository = userRepository;
            _passValidator = passValidator;
            _jwtValidator = jwtValidator;
            _logger = logger;
        }

        // POST api/registration
        [HttpPost("registration")]
        public async Task<ActionResult> RegisterUser([FromBody] FullUser user)
        {
            try
            {
                if (_passValidator.ValidateUser(user) is not null) return BadRequest("User already in DB");
            }
            catch (ArgumentException ex)
            {
                if (ex.Message == "Password is not match.") return BadRequest("User already in DB");
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
            await _userRepository.AddUserAsync(user, user.Password);
            return Ok();
        }
        // POST api/signin
        [HttpPost("signin")]
        public ActionResult SignInUser([FromBody] FullUser us)
        {
            IClientUser clientUser;
            IUser? user;
            try
            {
                if (us.Login is null) return BadRequest("Invalid Login");
                if (us.Password is null) return BadRequest("Invalid Password");
                clientUser = _passValidator.GetConverter().CreateUser(new(us.Login, us.Password));
                user = _passValidator.ValidateUser(clientUser);
                if (user == null) return BadRequest("User with this login is not created");
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
            return Ok(new { Token = _userRepository.SignInUser(user) });
        }
        // POST api/passChange       
        [HttpPost("passChange")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult> ChangeUserPassword([FromBody] ReceivingChangePassword pasCh)
        {
            IUser? user;
            IClientUser clientUser;
            if (pasCh.OldPassword is null) return BadRequest("Invalid Old Password");
            if (pasCh.NewPassword is null) return BadRequest("Invalid New Password");
            if (!_passValidator.ValidateCredential(pasCh.NewPassword)) return BadRequest("Invalid New Password");
            if (!_passValidator.ValidateCredential(pasCh.OldPassword)) return BadRequest("Invalid Old Password");
            try
            {
                clientUser = _jwtValidator.GetConverter().CreateUser(User);
                var us = new FullUser(ObjectId.Empty, clientUser.Login, pasCh.OldPassword);
                user = _passValidator.ValidateUser(us);
                if (user is null) return Unauthorized();
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex.Message);
                if (ex.Message == "Password is not match.") return BadRequest("Wrong old Password");
                return BadRequest(ex.Message);
            }
            await _userRepository.ChangeUserPasswordAsync(user, pasCh.NewPassword);
            return Accepted();
        }
        // POST api/delUser          
        [HttpPost("delUser")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult> DeleteUser([FromBody] ReceivingPassword pas)
        {
            IClientUser clientUser;
            IUser? user;
            try
            {
                clientUser = _jwtValidator.GetConverter().CreateUser(User);
                clientUser = _passValidator.GetConverter().CreateUser(new(clientUser.Login, pas.Password));
                user = _passValidator.ValidateUser(clientUser);
                if (user == null) return Unauthorized();
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
            await _userRepository.DeleteUserAsync(user);
            return Accepted();
        }
    }
}
