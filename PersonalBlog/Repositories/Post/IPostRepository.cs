using PersonalBlog.Models;

namespace PersonalBlog.Repositories;

public interface IPostRepository
{
    
    Task<Post> CreatePostAsync(Post post);
    Task<bool> UpdatePostAsync(Post post);
    Task<bool> DeletePostAsync(long id);
    Task<Post> FindPostAsync(long id);
    Task<List<Post>> GetAllPostAsync();
}