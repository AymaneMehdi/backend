using ServicesDomicile.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IUserImageRepository
{
    Task<IEnumerable<UserImage>> GetAllUserImagesAsync();
    Task<UserImage> GetByIdAsync(Guid id);
    Task AddUserImageAsync(UserImage userImage);
    Task UpdateUserImageAsync(UserImage userImage);
    Task DeleteUserImageAsync(Guid id);
    Task<bool> UserImageExistsAsync(Guid id);
}
