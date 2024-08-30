using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ServicesDomicile.DTOs;
using ServicesDomicile.DTOs.OtherObjects;
using ServicesDomicile.Entities;
using ServicesDomicile.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ServicesDomicile.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GigsController : ControllerBase
    {
        private readonly IGigRepository _gigRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;

        public GigsController(IGigRepository gigRepository, UserManager<ApplicationUser> userManager, IConfiguration configuration, IWebHostEnvironment environment)
        {
            _gigRepository = gigRepository;
            _userManager = userManager;
            _configuration = configuration;
            _environment = environment;
        }

        [Authorize(Roles = StaticUserRoles.ADMIN)]
        [HttpPost]
        public async Task<ActionResult<Gig>> CreateGig([FromForm] CreateGigDto gigDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return BadRequest("Invalid User ID");
            }

            // Ensure the upload directory exists
            var uploadsRootFolder = _environment.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var uploadsFolder = Path.Combine(uploadsRootFolder, "uploads");

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Handle file uploads
            var images = new List<string>();
            var coverImagePath = string.Empty;
            if (gigDto.CoverImage != null)
            {
                coverImagePath = Path.Combine("uploads", Guid.NewGuid().ToString() + Path.GetExtension(gigDto.CoverImage.FileName));
                var fullPath = Path.Combine(uploadsRootFolder, coverImagePath);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await gigDto.CoverImage.CopyToAsync(stream);
                }
            }

            foreach (var file in gigDto.Images)
            {
                var filePath = Path.Combine("uploads", Guid.NewGuid().ToString() + Path.GetExtension(file.FileName));
                var fullPath = Path.Combine(uploadsRootFolder, filePath);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                images.Add(filePath);
            }

            var gig = new Gig
            {
                Id = Guid.NewGuid(),
                Title = gigDto.Title,
                UserId = userId,
                User = user,
                Images = images,
                Desc = gigDto.Desc,
                ShortTitle = gigDto.ShortTitle,
                ShortDesc = gigDto.ShortDesc,
                CoverImage = coverImagePath, // Save the relative path
                CategoryId = gigDto.CategoryId
            };

            await _gigRepository.AddGigAsync(gig);
            return CreatedAtAction(nameof(GetGig), new { id = gig.Id }, gig);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Gig>>> GetGigs()
        {
            var gigs = await _gigRepository.GetAllGigsAsync();
            return Ok(gigs);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Gig>> GetGig(Guid id)
        {
            var gig = await _gigRepository.GetGigByIdAsync(id);

            if (gig == null)
            {
                return NotFound();
            }

            return gig;
        }

        [Authorize(Roles = StaticUserRoles.ADMIN)]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGig(Guid id, [FromBody] UpdateGigDto gigDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var gig = await _gigRepository.GetGigByIdAsync(id);

            if (gig == null)
            {
                return NotFound();
            }

            if (gig.UserId != userId)
            {
                return Forbid();
            }

            gig.Title = gigDto.Title;
            gig.Desc = gigDto.Desc;
            gig.ShortTitle = gigDto.ShortTitle;
            gig.ShortDesc = gigDto.ShortDesc;
            gig.CategoryId = gigDto.CategoryId;

            await _gigRepository.UpdateGigAsync(gig);
            return NoContent();
        }

        [Authorize(Roles = StaticUserRoles.ADMIN)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGig(Guid id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var gig = await _gigRepository.GetGigByIdAsync(id);

            if (gig == null)
            {
                return NotFound();
            }

            if (gig.UserId != userId)
            {
                return Forbid();
            }

            await _gigRepository.DeleteGigAsync(id);
            return NoContent();
        }
    }
}
