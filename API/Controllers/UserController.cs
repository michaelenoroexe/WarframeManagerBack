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
    /// <summary>
    /// <para>Route: api</para>
    /// Contoller to handle user requests, such as:
    ///  <para>- Create new user;</para>
    ///  <para>- Sign user in;</para>
    ///  <para>- Change user password;</para>
    ///  <para>- Delete user.</para>
    /// </summary>
    [Route("api")]
    [ApiController]
    public sealed class UserController : ControllerBase
    {
        private readonly IUserManager _userRepository;
        private readonly IUserValidator<(string, string)> _passValidator;
        private readonly IUserValidator<ClaimsPrincipal> _jwtValidator;
        private readonly ILogger<UserController> _logger;
        /// <summary>
        /// Initialize controller to handle user administration requests.
        /// </summary>
        /// <param name="userRepository">Repository to handle main part of requests logic.</param>
        /// <param name="passValidator">Validator to validate user from Login/Password input.</param>
        /// <param name="jwtValidator">Validator to validate user from JWT token.</param>
        /// <param name="logger">Logger to save information about errors.</param>
        public UserController(IUserManager userRepository, IUserValidator<(string, string)> passValidator,
                              IUserValidator<ClaimsPrincipal> jwtValidator, ILogger<UserController> logger)
        {
            _userRepository = userRepository;
            _passValidator = passValidator;
            _jwtValidator = jwtValidator;
            _logger = logger;
        }
        /// <summary>
        /// POST api/registration
        /// <param>Add new user to DB.</param>
        /// </summary>
        /// <param name="user">User information about new user.</param>
        /// <returns>Status of request.</returns>
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
        /// <summary>
        /// POST api/signin
        /// <param>Create JWT token on valid user and send back to client.</param>
        /// </summary>
        /// <param name="us">User information about user to login.</param>
        /// <returns>User JWT token.</returns>
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
        /// <summary>
        /// POST api/passChange
        /// <param>Change user password stored in DB.</param>
        /// </summary>
        /// <param name="pasCh">User old and new password in additional to JWT token.</param>
        /// <returns>Status of request.</returns>
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
        /// <summary>
        /// POST api/delUser
        /// <param>Delete user from database.</param>
        /// </summary>
        /// <param name="pas">Password for the deleted account.</param>
        /// <returns></returns>
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
