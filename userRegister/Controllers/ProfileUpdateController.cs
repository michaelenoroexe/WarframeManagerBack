using API.Models;
using API.Models.Interfaces;
using API.Models.UserWork.Setter;
using API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Shared;
using System.Security.Claims;
using UserValidation;

namespace API.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/ProfUp")]
    [ApiController]
    internal class ProfileUpdateController : ControllerBase
    {
        private IChangeData _repository;
        private readonly ILogger<GetDataController> _logger;
        private readonly IUserValidator<ClaimsPrincipal> _validator;
        private readonly IUserConverter<ClaimsPrincipal> _converter;

        private IUser ValidateUser(ClaimsPrincipal us)
        {
            IUser? user = null;
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

        public ProfileUpdateController(IChangeData dataRepository, IUserValidator<ClaimsPrincipal> validator, ILogger<GetDataController> logger)
        {
            _repository = dataRepository;
            _logger = logger;
            _validator = validator;
            _converter = _validator.GetConverter();
        }

        // POST api/ProfUp
        [HttpPost]
        public ActionResult Post([FromBody] ResourceChange res)
        {
            IUser user;
            try
            {
                user = ValidateUser(User);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            if (res.Type == "resource")
                try
                {
                    _repository.UpdateResource(user, new KeyValuePair<string, int>(res.Resource, res.Number));
                    return Accepted();
                }
                catch (Exception ex)
                {
                    _logger.LogInformation($"User:{user.Login}, catch exeption on resource update:" + ex.Message, ex);
                    return BadRequest();
                } 
            if (res.Type == "item")
                try
                {
                    _repository.UpdateItem(user, new KeyValuePair<string, int>(res.Resource, res.Number));
                    return Accepted();
                }
                catch (Exception ex)
                {
                    _logger.LogInformation($"User:{user.Login}, catch exeption on item update:" + ex.Message, ex);
                    return BadRequest();
                }
            return BadRequest();

        }
        // POST api/ProfUp/creds
        [HttpPost("creds")]
        public ActionResult CredCh([FromBody] Cred num)
        {
            IUser? user = null;
            try
            {
                try
                {
                    user = ValidateUser(User);
                }
                catch (ArgumentException ex)
                {
                    return BadRequest(ex.Message);
                }

                _repository.UpdateCredits(user, num.Number);
                return Accepted();
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"User:{user?.Login}, catch exeption on item update:" + ex.Message, ex);
                return BadRequest();
            }  
        }
        // POST api/ProfUp/userInfo
        [HttpPost("userInfo")]
        public ActionResult ProfCh([FromBody] UserInfo ch)
        {
            IUser? user = null;
            try
            {
                try
                {
                    user = ValidateUser(User);
                }
                catch (ArgumentException ex)
                {
                    return BadRequest(ex.Message);
                }

                _repository.UpdateProfile(user, ch);
                return Accepted();
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"User:{user?.Login}, catch exeption on item update:" + ex.Message, ex);
                return BadRequest();
            }
        }
    }
}
