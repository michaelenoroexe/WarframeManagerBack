using API.Models.Common.ItemComp;
using API.Models.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared;
using System.Security.Claims;
using UserValidation;

namespace API.Controllers
{
    [Route("api/GetData")]
    [ApiController]
    public sealed class GetDataController : ControllerBase
    {
        private readonly IGetData _repository;
        private readonly ILogger<GetDataController> _logger;
        private readonly IUserValidator<ClaimsPrincipal> _validator;
        private readonly IUserConverter<ClaimsPrincipal> _converter;

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
                _logger.LogError("User token dont have login:" + ex.Message);
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

        public GetDataController(IGetData dataRepository, IUserValidator<ClaimsPrincipal> validator, ILogger<GetDataController> logger)
        {
            _repository = dataRepository;
            _logger = logger;
            _validator = validator;
            _converter = _validator.GetConverter();
        }

        [HttpGet("ResourcesList")] public ActionResult GetResourcesList() => Ok(_repository.GetResourcesList().Cast<Resource>());
        [HttpGet("ItemsList")] public ActionResult GetItemsList() => Ok(_repository.GetItemsList().Cast<Item>());
        [HttpGet("TypesList")] public async Task<ActionResult> GetTypesList() => Ok(await _repository.GetTypesListAsync());
        [HttpGet("Planets")] public async Task<ActionResult> GetPlanets() => Ok(await _repository.GetPlanetListAsync());
        // GET: api/GetData/UserResourcesList
        [HttpGet("UserResourcesList")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult> GetUserResourcesList()
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

            IEnumerable<IResource> res = await _repository.GetUserResourcesAsync(user);
            return Ok(res.Cast<Item>());
        }
        // GET: api/GetData/UserItemsList
        [HttpGet("UserItemsList")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult> GetUserItemsList()
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

            IEnumerable<IResource> res = await _repository.GetUserItemsAsync(user);
            return Ok(res.Cast<Item>());
        }
        // GET: api/GetData/UserCredits
        [HttpGet("UserCredits")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult> GetUserCredits()
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

            int res = await _repository.GetUserCreditsAsync(user);
            return Ok(res);
        }
        // GET: api/GetData/UserInfo
        [HttpGet("UserInfo")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult> GetUserInfo()
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

            UserInfo res = await _repository.GetUserInfoAsync(user);
            return Ok(res);
        }
    }
}
