using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ServicesDomicile.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RolesController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateRole([FromBody] string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
            {
                return BadRequest("Role name cannot be empty");
            }

            bool roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (roleExists)
            {
                return Conflict($"Role '{roleName}' already exists");
            }

            IdentityRole newRole = new IdentityRole(roleName.ToUpper());
            IdentityResult result = await _roleManager.CreateAsync(newRole);

            if (result.Succeeded)
            {
                return Ok($"Role '{roleName}' created successfully");
            }

            return StatusCode(500, "Internal server error");
        }

        [HttpGet]
        [Route("all-roles")]
        public IActionResult GetAllRoles()
        {
            var roles = _roleManager.Roles.ToList();
            return Ok(roles);
        }
    }
}
