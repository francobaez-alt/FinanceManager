using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using API.Extensions;
using API.Authorization;
using System.Security.Claims;
using Application.Interfaces.Repositories;
using Application.Common;

namespace API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
       
        private IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService; 
        }

        [HttpGet]
        //[HasPermission("Users.Read")]
        public async Task<IActionResult> GetMyUser()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (claim == null)
                return Unauthorized();
            var user = await _userService.GetByIdAsync(int.Parse(claim.Value));

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);    
        }


    }
}