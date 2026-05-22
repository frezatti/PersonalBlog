using PersonalBlog.Models;

namespace PersonalBlog.Repositories;

public interface IUserRepository
{
    Task<User> CreateUserAsync(User user);
    Task<bool> UpdateUserAsync(User user);
    Task<bool> DeleteUserAsync(long id);
    Task<User> FindUserAsync(long id);
    Task<List<User>> GetAllUserAsync();
}