using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FlashCardsAPI.Models.DB;
using System.Collections.Generic;
using FlashCardsAPI.Services;

namespace FlashCardsAPI.Controllers
{
    [Produces("application/json")]
    [Authorize]
    [Route("api/Auth")]
    public class AuthController : Controller
    {
        private IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/Auth
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "validated", "correctly" };
        }

        //// GET: api/Auth/5
        //[HttpGet("{id}", Name = "Get")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST: api/Auth
        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Post([FromBody]Users userParam)
        {
            var user = _userService.Authenticate(userParam.UserName, userParam.Password);
            if (user == null)
            {
                return BadRequest(new { message = "Username or password could not be validated!" });
            }
            return Ok(user); // or just return token??
        }
        
        //// PUT: api/Auth/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}
        
        //// DELETE: api/ApiWithActions/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
