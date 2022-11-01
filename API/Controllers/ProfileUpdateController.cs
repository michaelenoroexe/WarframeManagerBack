using API.Models.UserWork.Setter;
using API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared;
using System.Security.Claims;
using UserValidation;

namespace API.Controllers
{
    /// <summary>
    /// <para>Route: api/ProfUp</para>
    /// Contoller to handle user change data requests, such as:
    ///  <para>- Change user owned number for any resource;</para>
    ///  <para>- Change user owned number for any item;</para>
    ///  <para>- Change user credits number;</para>
    ///  <para>- Change user profile information.</para>
    /// </summary>
    [ApiController]
    [Route("api/ProfUp")]   
    [Authorize(AuthenticationSchemes = "Bearer")]
    public sealed class ProfileUpdateController : ControllerBase
    {
        private readonly IChangeData _repository;
        private readonly ILogger<GetDataController> _logger;
        private readonly IUserValidator<ClaimsPrincipal> _validator;
        private readonly IUserConverter<ClaimsPrincipal> _converter;
        /// <summary>
        /// Function to validate user by JWT token.
        /// </summary>
        /// <param name="us">User login in claims.</param>
        /// <returns>Full user for the program to work with it.</returns>
        private IUser ValidateUser(ClaimsPrincipal us)
        {
            IUser? user;
            IClientUser? clientUser;
            try
            {
                clientUser = _converter.CreateUser(us);
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError("User tocken dont have login:" + ex.Message);
                throw;
            }
            try
            {
                user = _validator.ValidateUser(clientUser);
                if (user is null) throw new ArgumentException("User is not in database.");
            }
            catch (ArgumentException)
            {
                throw;
            }
            return user;
        }
        /// <summary>
        /// Initialize controller to handle user change data requests.
        /// </summary>
        /// <param name="dataRepository">Repository to handle main part of requests logic.</param>
        /// <param name="validator">Validator to validate user from JWT token.</param>
        /// <param name="logger">Logger to save information about errors.</param>
        public ProfileUpdateController(IChangeData dataRepository, IUserValidator<ClaimsPrincipal> validator, ILogger<GetDataController> logger)
        {
            _repository = dataRepository;
            _logger = logger;
            _validator = validator;
            _converter = _validator.GetConverter();
        }
        /// <summary>
        /// POST api/ProfUp
        /// <para>Handle requests to change resource number, receive data in format:</para>
        /// <para>- Resource id;</para>
        /// <para>- New owned number;</para>
        /// <para>- Type (item or resource).</para>
        /// </summary>
        /// <param name="res">Object received user request data.</param>
        /// <returns>Returns request accepted or not.</returns>
        [HttpPost("")]
        public ActionResult ChangeResourceNumber([FromBody] ReceiveResourceChange res)
        {
            IUser user;
            try { user = ValidateUser(User); }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            Action<IUser, KeyValuePair<string, int>>? update = null;
            if (res.Type == "resource") update = _repository.UpdateResource;
            if (res.Type == "item") update = _repository.UpdateItem;
            if (update is null) return BadRequest("Wrong item type.");
            try
            {
                update(user, new KeyValuePair<string, int>(res.Resource, res.Number));
                return Accepted();
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"User:{user.Login}, catch exeption on resource update:" + ex.Message, ex);
                return BadRequest();
            }
        }
        /// <summary>
        /// POST api/ProfUp/creds
        /// <para>Handle requests to change user credits number.</para>
        /// </summary>
        /// <param name="num">Object with field int to receive neww user number of credits.</param>
        /// <returns>Returns request accepted or not.</returns>
        [HttpPost("creds")]
        public ActionResult ChangeCreditNumber([FromBody] ReceiveCreditsChange num)
        {
            IUser? user;
            try { user = ValidateUser(User); }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            _repository.UpdateCredits(user, num.Number);
            return Accepted();
        }
        /// <summary>
        /// POST api/ProfUp/userInfo
        /// <para>Handle requests to change user profile information</para>
        /// </summary>
        /// <param name="ch">Object with user profile information.</param>
        /// <returns>Returns request accepted or not.</returns>
        [HttpPost("userInfo")]
        public ActionResult ChangeProfileInfo([FromBody] UserInfo ch)
        {
            IUser? user;
            try { user = ValidateUser(User); }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            _repository.UpdateProfile(user, new UserInfo(user, ch.Rank, ch.Image));
            return Accepted();
        }
    }
}
