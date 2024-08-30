using ServicesDomicile.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IUserRepository
{
    Task<ApplicationUser> FindByNameAsync(string userName);
    Task AddToRoleAsync(ApplicationUser user, string role);
    Task RemoveFromRoleAsync(ApplicationUser user, string role);
    Task AddUserImageAsync(UserImage userImage);
    Task AddUserCategoriesAsync(IEnumerable<UserCategory> userCategories);
    Task SaveChangesAsync();
}
