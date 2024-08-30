using Microsoft.EntityFrameworkCore;
using ServicesDomicile.Db_Context;
using ServicesDomicile.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

public class UserImageRepository : IUserImageRepository
{
    private readonly AppDbContext _context;

    public UserImageRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<UserImage>> GetAllUserImagesAsync()
    {
        return await _context.UserImages.ToListAsync();
    }

    public async Task<UserImage> GetByIdAsync(Guid id)
    {
        return await _context.UserImages.FindAsync(id);
    }

    public async Task AddUserImageAsync(UserImage userImage)
    {
        _context.UserImages.Add(userImage);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateUserImageAsync(UserImage userImage)
    {
        _context.UserImages.Update(userImage);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteUserImageAsync(Guid id)
    {
        var userImage = await _context.UserImages.FindAsync(id);
        if (userImage != null)
        {
            _context.UserImages.Remove(userImage);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> UserImageExistsAsync(Guid id)
    {
        return await _context.UserImages.AnyAsync(ui => ui.Id == id);
    }
}
