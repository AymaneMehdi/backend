using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServicesDomicile.DTOs.OtherObjects;
using ServicesDomicile.DTOs.Owner;
using ServicesDomicile.Entities;
using System.Data;

namespace ServicesDomicile.Controllers.Owner
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoriesController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetAllCategories()
        {
            var categories = await _categoryRepository.GetAllCategoriesAsync();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategoryById(Guid id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        [Authorize(Roles = StaticUserRoles.OWNER)]
        [HttpPost]
        public async Task<ActionResult<Category>> CreateCategory([FromBody] CreateCategoryDto createCategoryDto)
        {
            var category = new Category
            {
                Id = Guid.NewGuid(),
                Name = createCategoryDto.name
            };

            await _categoryRepository.AddCategoryAsync(category);
            return CreatedAtAction(nameof(GetCategoryById), new { id = category.Id }, category);
        }

        [Authorize(Roles = StaticUserRoles.OWNER)]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(Guid id, [FromBody] UpdateCategoryDto category)
        {

            var existingCategory = await _categoryRepository.GetByIdAsync(id);
            if (existingCategory == null)
            {
                return NotFound();
            }
            existingCategory.Name = category.name;
            await _categoryRepository.UpdateCategoryAsync(existingCategory);
            return NoContent();
        }

        [Authorize(Roles = StaticUserRoles.OWNER)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            if (!await _categoryRepository.CategoryExistsAsync(id))
            {
                return NotFound();
            }

            await _categoryRepository.DeleteCategoryAsync(id);
            return NoContent();
        }
    }
}