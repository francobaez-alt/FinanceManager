using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using API.Extensions;
using API.Authorization;
using System.Security.Claims;
using Application.Interfaces.Repositories;
using Application.Common;
using Application.DTOs.Users;

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

       
        [HttpGet("me")]
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
        [HttpPut("me/password")]
        public async Task<IActionResult> UpdateMyPassword([FromBody] UpdatePasswordDto updatePasswordDto)
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (claim == null)
                return Unauthorized();
            var Result= await _userService.UpdatePasswordAsync(int.Parse(claim.Value), updatePasswordDto);
            
            if(!Result.Success)
            {
                return BadRequest(Result);
            }

            return Ok(Result);
        }

        [HttpGet("list")]
        [HasPermission("Users.Read")]
        public async Task<IActionResult> ListUsers()
        {
            var users = await _userService.ListUsersAsync();
            return Ok(users);
        }

        [HttpPut("activate/{id}")]
        [HasPermission("Users.Update")]
        public async Task<IActionResult> ActivateUser(int id)
        {
            var result = await _userService.ActivateUserAsync(id);
            if (!result.Success)
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        [HttpPut("desactivate/{id}")]
        [HasPermission("Users.Update")]
        public async Task<IActionResult> DesactivateUser(int id)
        {
            var result = await _userService.DesactiveUserAsync(id);
            if (!result.Success)
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        [HttpPut("me/update")]
        public async Task<IActionResult> UpdateMyUser([FromBody] UpdateUserDto updateUserDto)
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (claim == null)
                return Unauthorized();

            var result = await _userService.UpdateUserAsync(int.Parse(claim.Value), updateUserDto);
            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

    }
}