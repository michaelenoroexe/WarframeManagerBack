using API.Models.Common.ItemComp;
using API.Models.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared;
using System.Security.Claims;
using UserValidation;

namespace API.Controllers
{
    /// <summary>
    /// <para>Route: api/GetData</para>
    /// Contoller to handle user getting data requests, return such information as:
    ///  <para>- List of all items with 0 in owned for each;</para>
    ///  <para>- List of all resources with 0 in owned for each;</para>
    ///  <para>- List of all planets in format {PlanetID}:{PlanetName};</para>
    ///  <para>- List of all resources types;</para>
    ///  <para>- List of all items with number in "owned" that user have at this time;</para>
    ///  <para>- List of all resources with number in "owned" that user have at this time;</para>
    ///  <para>- Number of user credits;</para>
    ///  <para>- Users profile information.</para>
    /// </summary>
    [ApiController]
    [Route("api/GetData")]    
    public sealed class GetDataController : ControllerBase
    {
        private readonly IGetData _repository;
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
        /// <summary>
        /// Initialize controller to handle user getting data requests.
        /// </summary>
        /// <param name="dataRepository">Repository to handle main part of requests logic.</param>
        /// <param name="validator">Validator to validate user from JWT token.</param>
        /// <param name="logger">Logger to save information about errors.</param>
        public GetDataController(IGetData dataRepository, IUserValidator<ClaimsPrincipal> validator, ILogger<GetDataController> logger)
        {
            _repository = dataRepository;
            _logger = logger;
            _validator = validator;
            _converter = _validator.GetConverter();
        }
        /// <summary>
        /// GET: api/GetData/ResourcesList
        /// </summary>
        /// <returns>List of all resources with 0 in owned number.</returns>
        [HttpGet("ResourcesList")] public ActionResult GetResourcesList() => Ok(_repository.GetResourceList().Cast<Resource>());
        /// <summary>
        /// GET: api/GetData/ItemsList
        /// </summary>
        /// <returns>List of all items with 0 in owned number.</returns>
        [HttpGet("ItemsList")] public ActionResult GetItemsList() => Ok(_repository.GetItemList().Cast<Item>());
        /// <summary>
        /// GET: api/GetData/TypesList
        /// </summary>
        /// <returns>List of all item types.</returns>
        [HttpGet("TypesList")] public async Task<ActionResult> GetTypesList() => Ok(await _repository.GetTypeListAsync());
        /// <summary>
        /// GET: api/GetData/Planets
        /// </summary>
        /// <returns>List of all planets.</returns>
        [HttpGet("Planets")] public async Task<ActionResult> GetPlanets() => Ok(await _repository.GetPlanetListAsync());
        /// <summary>
        /// GET: api/GetData/UserResourcesList
        /// </summary>
        /// <returns>List of all resources with number in "owned" that user have at this time.</returns>
        [HttpGet("UserResourcesList")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult> GetUserResourcesList()
        {
            IUser user;
            try { user = ValidateUser(User); }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            IEnumerable<IResource> res = await _repository.GetUserResourcesAsync(user);
            return Ok(res.Cast<Item>());
        }
        /// <summary>
        /// GET: api/GetData/UserItemsList
        /// </summary>
        /// <returns>List of all resources with number in "owned" that user have at this time.</returns>
        [HttpGet("UserItemsList")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult> GetUserItemsList()
        {
            IUser user;
            try { user = ValidateUser(User); }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            IEnumerable<IResource> res = await _repository.GetUserItemsAsync(user);
            return Ok(res.Cast<Item>());
        }
        /// <summary>
        /// GET: api/GetData/UserCredits
        /// </summary>
        /// <returns>Number of user credits.</returns>
        [HttpGet("UserCredits")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult> GetUserCredits()
        {
            IUser user;
            try
            { user = ValidateUser(User); }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            int res = await _repository.GetUserCreditsAsync(user);
            return Ok(res);
        }
        /// <summary>
        /// GET: api/GetData/UserInfo
        /// </summary>
        /// <returns>Users profile information.</returns>
        [HttpGet("UserInfo")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult> GetUserInfo()
        {
            IUser user;
            try
            { user = ValidateUser(User); }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            UserInfo res = await _repository.GetUserInfoAsync(user);
            return Ok(res);
        }
    }
}
