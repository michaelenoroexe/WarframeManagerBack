﻿using Microsoft.AspNetCore.Mvc;
using API.Repositories;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using MongoDB.Bson;
using API.Models.Responses;


namespace API.Controllers
{
    [Route("api/GetData")]
    [ApiController]
    public class GetDataController : ControllerBase
    {
        private GetDataRepository _repository;
        private readonly ILogger<GetDataController> _logger;

        public GetDataController(GetDataRepository dataRepository, ILogger<GetDataController> logger)
        {
            _repository = dataRepository;
            _logger = logger;
        }
        // GET: api/GetData/UserResourcesList
        [HttpGet("UserResourcesList")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult> GetUserResourcesList()
        {
            var user = await JwtAuthentication.GetUserFromTokenAsync(HttpContext.User.Claims.FirstOrDefault().Value);
            if (user == null) return NotFound("User not found");
            var changes = UserResourcesChangesBuffer._totalBuffer.FirstOrDefault(userChan => userChan.User == user.Id);
            GetDataResponses res = await _repository.GetUserItAsync(_repository.GetResourcesListAsync, _repository.GetUsersResourcesAsync, user, changes?.Resources);
            if (res.Code == 20) return Ok(res.Data);
            return BadRequest(res.Data);
        }

        [HttpGet("ResourcesList")]
        public async Task<ActionResult> GetResourcesList()
        {
            //Return full list of resources
            return Ok(await _repository.GetResourcesListAsync());
        }

        [HttpGet("UserItemsList")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult> GetUserItemsList()
        {
            var user = await JwtAuthentication.GetUserFromTokenAsync(HttpContext.User.Claims.FirstOrDefault().Value);
            if (user == null) return NotFound("User not found");
            var changes = UserResourcesChangesBuffer._totalBuffer.FirstOrDefault(userChan => userChan.User == user.Id);
            GetDataResponses res = await _repository.GetUserItAsync(_repository.GetItemsListAsync, _repository.GetUsersItemsAsync, user, changes?.Items);
            if (res.Code == 20) return Ok(res.Data);
            return BadRequest(res.Data);
        }

        [HttpGet("ItemsList")]
        public async Task<ActionResult> GetItemsList()
        {
            // Return full list of components
            return Ok(await _repository.GetItemsListAsync());
        }

        [HttpGet("TypesList")]
        public async Task<ActionResult> GetTypesList()
        {
            // Return full list of components
            return Ok(await _repository.GetTypesListAsync());
        }

        [HttpGet("Planets")]
        public async Task<ActionResult> GetPlanets()
        {
            return Ok(await _repository.GetPlanetListAsync());
        }
      
        [HttpGet("UserCredits")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult> GetUserCredits()
        {
            var user = await JwtAuthentication.GetUserFromTokenAsync(HttpContext.User.Claims.FirstOrDefault().Value);
            if (user == null) return NotFound("User not found");
            Task<int> res = _repository.GetUserCredits(user);
            var changes = UserResourcesChangesBuffer._totalBuffer.FirstOrDefault(userChan => userChan.User == user.Id);
            if (changes?.Credits is not null && changes.Credits != 0) return Ok(changes.Credits);
            
             return Ok(await res);
        }

        // GET: api/GetData/UserInfo
        [HttpGet("UserInfo")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult> GetUserInfo()
        {
            var user = await JwtAuthentication.GetUserFromTokenAsync(HttpContext.User.Claims.FirstOrDefault().Value);
            if (user == null) return NotFound("User not found");
            Task<UserInfo> res = _repository.GetUserInfo(user);
            var changes = UserResourcesChangesBuffer._totalBuffer.FirstOrDefault(userChan => userChan.User == user.Id);
            if (changes?.ProfInfo is not null) return Ok(changes.ProfInfo.WithoutId());
            var re = await res;
            return Ok(re.WithoutId());
        }
    }
}
