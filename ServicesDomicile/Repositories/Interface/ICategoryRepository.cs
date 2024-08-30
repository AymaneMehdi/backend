using ServicesDomicile.Entities;

public interface ICategoryRepository
{
    Task<IEnumerable<Category>> GetAllCategoriesAsync();
    Task<Category> GetByIdAsync(Guid id);
    Task AddCategoryAsync(Category category);
    Task UpdateCategoryAsync(Category category);
    Task DeleteCategoryAsync(Guid id);
    Task<bool> CategoryExistsAsync(Guid id);
}
