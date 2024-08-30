using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ServicesDomicile.DTOs;
using ServicesDomicile.Entities;

namespace ServicesDomicile.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        public readonly UserManager<ApplicationUser> _userManager;
        public readonly RoleManager<IdentityRole> _roleManager;
        public readonly IConfiguration _configuration;

        public UsersController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        [HttpGet]
        [Route("all-users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = _userManager.Users.ToList();
            return Ok(users);
        }

        [HttpGet]
        [Route("user/{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user is null)
                return NotFound("User not found");

            return Ok(user);
        }

        [HttpPut]
        [Route("update-user")]
        public async Task<IActionResult> UpdateUser([FromBody] RegisterDto updateUserDto)
        {
            var user = await _userManager.FindByNameAsync(updateUserDto.UserName);

            if (user is null)
                return NotFound("User not found");

            user.FirstName = updateUserDto.FirstName;
            user.LastName = updateUserDto.LastName;
            user.Email = updateUserDto.Email;
            user.UserName = updateUserDto.UserName;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return Ok("User updated successfully");
            }

            return StatusCode(500, "Internal server error");
        }

        [HttpDelete]
        [Route("delete-user/{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user is null)
                return NotFound("User not found");

            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                return Ok("User deleted successfully");
            }

            return StatusCode(500, "Internal server error");
        }

        [HttpPost]
        [Route("remove-role")]
        public async Task<IActionResult> RemoveRoleFromUser([FromBody] UpdatePermissionDto updatePermissionDto, [FromQuery] string role)
        {
            var user = await _userManager.FindByNameAsync(updatePermissionDto.UserName);

            if (user is null)
                return NotFound("User not found");

            var result = await _userManager.RemoveFromRoleAsync(user, role.ToUpper());

            if (result.Succeeded)
            {
                return Ok($"Role '{role}' removed from user '{updatePermissionDto.UserName}' successfully");
            }

            return StatusCode(500, "Internal server error");
        }
    }
}
