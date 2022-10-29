﻿using Microsoft.AspNetCore.Mvc;
using API.Repositories;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using MongoDB.Bson;
using API.Models.Responses;
using API.Models.Interfaces;
using UserValidation;
using System.Security.Claims;
using Shared;
using API.Models.Common.ItemComp;

namespace API.Controllers
{
    [Route("api/GetData")]
    [ApiController]
    public class GetDataController : ControllerBase
    {
        private IGetData _repository;
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
                _logger.LogError("User tocken dont have login:"+ex.Message);
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

        [HttpGet("ResourcesList")]
        public ActionResult GetResourcesList() => Ok(_repository.GetResourcesList());
        [HttpGet("ItemsList")]
        public ActionResult GetItemsList() => Ok(_repository.GetItemsList());
        [HttpGet("TypesList")]
        public async Task<ActionResult> GetTypesList() => Ok(await _repository.GetTypesListAsync());
        [HttpGet("Planets")]
        public async Task<ActionResult> GetPlanets() => Ok(await _repository.GetPlanetListAsync());

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
            return Ok(res);
        }

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
            return Ok(res);
        }
      
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
