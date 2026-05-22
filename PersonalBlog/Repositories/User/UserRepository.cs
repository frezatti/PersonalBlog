using Microsoft.EntityFrameworkCore;
using PersonalBlog.Data;
using PersonalBlog.Models;

namespace PersonalBlog.Repositories;

public class UserRepository(AppDBContext context) : IUserRepository
{
    public async Task<User> CreateUserAsync(User user)
    {
        context.Users.Add(user);
        await context.SaveChangesAsync();

        return user;
    }

    public async Task<bool> UpdateUserAsync(User user)
    {
        var result = await context.Users
            .Where(x => x.Id == user.Id)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(dbUser => dbUser.Name, user.Name)
                .SetProperty(dbUser => dbUser.Email, user.Email));

        return result > 0;
    }

    public async Task<bool> DeleteUserAsync(long id)
    {
        var result = await context.Users
            .Where(user => user.Id == id)
            .ExecuteDeleteAsync();

        return result > 0;
    }

    public async Task<User> FindUserAsync(long id)
    {
        var user = await context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id)
                   ?? throw new KeyNotFoundException("User not found.");

        return user;
    }

    public async Task<List<User>> GetAllUserAsync()
    {
        return await context.Users.AsNoTracking().ToListAsync();
    }
}