using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ServicesDomicile.Db_Context;
using ServicesDomicile.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

public class UserRepository : IUserRepository
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly AppDbContext _context;

    public UserRepository(UserManager<ApplicationUser> userManager, AppDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    public async Task<ApplicationUser> FindByNameAsync(string userName)
    {
        return await _userManager.FindByNameAsync(userName);
    }

    public async Task AddToRoleAsync(ApplicationUser user, string role)
    {
        await _userManager.AddToRoleAsync(user, role);
    }

    public async Task RemoveFromRoleAsync(ApplicationUser user, string role)
    {
        await _userManager.RemoveFromRoleAsync(user, role);
    }

    public async Task AddUserImageAsync(UserImage userImage)
    {
        await _context.UserImages.AddAsync(userImage);
    }

    public async Task AddUserCategoriesAsync(IEnumerable<UserCategory> userCategories)
    {
        await _context.UserCategories.AddRangeAsync(userCategories);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
