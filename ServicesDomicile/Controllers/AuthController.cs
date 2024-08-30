using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ServicesDomicile.Db_Context;
using ServicesDomicile.DTOs;
using ServicesDomicile.DTOs.OtherObjects;
using ServicesDomicile.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ServicesDomicile.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public readonly UserManager<ApplicationUser> _userManager;
        public readonly RoleManager<IdentityRole> _roleManager;
        public readonly IConfiguration _configuration;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IWebHostEnvironment _environment;
        private readonly IUserRepository _userRepository;
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration,
            ICategoryRepository categoryRepository,
            IWebHostEnvironment environment,
            IUserRepository userRepository)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _categoryRepository = categoryRepository;
            _environment = environment;
            _userRepository = userRepository;
            _context = context;
        }

        [HttpPost]
        [Route("seed-roles")]
        public async Task<IActionResult> SeedRoles()
        {
            bool isOwnerRoleExists = await _roleManager.RoleExistsAsync(StaticUserRoles.OWNER);
            bool isAdminRoleExists = await _roleManager.RoleExistsAsync(StaticUserRoles.ADMIN);
            bool isUserRoleExists = await _roleManager.RoleExistsAsync(StaticUserRoles.USER);
            if (isOwnerRoleExists && isAdminRoleExists && isUserRoleExists)
            {
                return Ok("Roles Seeding is Already Done");
            }

            await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.OWNER));
            await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.ADMIN));
            await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.USER));

            return Ok("Role Seeding Done Successfully");
        }

        [HttpPost]
        [Route("remove-roles")]
        public async Task<IActionResult> RemoveRoles()
        {
            bool isOwnerRoleExists = await _roleManager.RoleExistsAsync(StaticUserRoles.OWNER);
            bool isAdminRoleExists = await _roleManager.RoleExistsAsync(StaticUserRoles.ADMIN);
            bool isUserRoleExists = await _roleManager.RoleExistsAsync(StaticUserRoles.USER);

            if (!isOwnerRoleExists && !isAdminRoleExists && !isUserRoleExists)
            {
                return Ok("Roles Removal is Already Done");
            }

            if (isOwnerRoleExists)
            {
                var ownerRole = await _roleManager.FindByNameAsync(StaticUserRoles.OWNER);
                await _roleManager.DeleteAsync(ownerRole);
            }

            if (isAdminRoleExists)
            {
                var adminRole = await _roleManager.FindByNameAsync(StaticUserRoles.ADMIN);
                await _roleManager.DeleteAsync(adminRole);
            }

            if (isUserRoleExists)
            {
                var userRole = await _roleManager.FindByNameAsync(StaticUserRoles.USER);
                await _roleManager.DeleteAsync(userRole);
            }

            return Ok("Roles Removal Done Successfully");
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            var isExistsUser = await _userManager.FindByNameAsync(registerDto.UserName);

            if (isExistsUser != null)
                return BadRequest("UserName Already Exists");

            ApplicationUser newUser = new ApplicationUser()
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                UserName = registerDto.UserName,
                PhoneNumber = registerDto.PhoneNumber,
                City = registerDto.City,
                SecurityStamp = Guid.NewGuid().ToString(),
            };

            var createUserResult = await _userManager.CreateAsync(newUser, registerDto.Password);

            if (!createUserResult.Succeeded)
            {
                var errorString = "User Creation Failed Because: ";
                foreach (var error in createUserResult.Errors)
                {
                    errorString += " # " + error.Description;
                }
                return BadRequest(errorString);
            }

            await _userManager.AddToRoleAsync(newUser, StaticUserRoles.USER);

            return Ok("User Created Successfully");
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.UserName);

            if (user is null)
                return Unauthorized("Invalid Credentials");

            var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, loginDto.Password);

            if (!isPasswordCorrect)
                return Unauthorized("Invalid Credentials");

            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim("JWTID", Guid.NewGuid().ToString()),
                new Claim("FirstName",user.FirstName),
                new Claim("LastName",user.LastName),
            };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var token = GenerateNewJsonWebToken(authClaims);

            return Ok(new { token, role = userRoles.FirstOrDefault() });
        }

        private string GenerateNewJsonWebToken(List<Claim> claims)
        {
            var authSecret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var tokenObject = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddHours(9),
                    claims: claims,
                    signingCredentials: new SigningCredentials(authSecret, SecurityAlgorithms.HmacSha256)
                );

            string token = new JwtSecurityTokenHandler().WriteToken(tokenObject);

            return token;
        }

        [HttpPost]
        [Route("make-admin")]
        public async Task<IActionResult> MakeAdmin([FromForm] UpdatePermissionDto updatePermissionDto)
        {
            var user = await _userManager.FindByNameAsync(updatePermissionDto.UserName);

            if (user == null)
                return BadRequest("Invalid User name");

            await _userManager.AddToRoleAsync(user, StaticUserRoles.ADMIN);
            await _userManager.RemoveFromRoleAsync(user, StaticUserRoles.USER);

            // Save the uploaded ID image
            if (updatePermissionDto.IdImage != null)
            {
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + updatePermissionDto.IdImage.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await updatePermissionDto.IdImage.CopyToAsync(fileStream);
                }

                var userImage = new UserImage
                {
                    Id = Guid.NewGuid(),
                    ImagePath = uniqueFileName,
                    UserId = user.Id
                };
                _context.UserImages.Add(userImage);
            }

            // Assign categories
            if (updatePermissionDto.CategoryIds != null && updatePermissionDto.CategoryIds.Any())
            {
                var userCategories = updatePermissionDto.CategoryIds.Select(categoryId => new UserCategory
                {
                    UserId = (user.Id),
                    CategoryId = categoryId
                });

                _context.UserCategories.AddRange(userCategories);
            }

            await _context.SaveChangesAsync();

            return Ok("User is now an ADMIN");
        }

        [HttpPost]
        [Route("make-owner")]
        public async Task<IActionResult> MakeOwner([FromBody] UpdatePermissionOwnerDto updatePermissionDto)
        {
            var user = await _userManager.FindByNameAsync(updatePermissionDto.UserName);

            if (user is null)
                return BadRequest("Invalid User name!!!!!!!!");

            await _userManager.AddToRoleAsync(user, StaticUserRoles.OWNER);
            await _userManager.RemoveFromRoleAsync(user, StaticUserRoles.USER);

            return Ok("User is now an Owner");
        }

        [HttpPost]
        [Route("validate-token")]
        public IActionResult ValidateToken([FromBody] string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]);

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _configuration["JWT:ValidIssuer"],
                    ValidAudience = _configuration["JWT:ValidAudience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = jwtToken.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;

                // Optionally: You can return more details about the token or the user here
                return Ok(new { userId, message = "Token is valid" });
            }
            catch (SecurityTokenException)
            {
                return Unauthorized("Token is invalid or expired");
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [Authorize]
        [HttpGet]
        [Route("secure-endpoint")]
        public IActionResult SecureEndpoint()
        {
            return Ok("You are authorized to access this endpoint.");
        }
    }
}
