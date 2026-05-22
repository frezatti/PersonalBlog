using Microsoft.EntityFrameworkCore;
using PersonalBlog.Data;
using PersonalBlog.Models;

namespace PersonalBlog.Repositories;

public class PostRepository(AppDBContext context) : IPostRepository
{

    public async Task<Post> CreatePostAsync(Post post)
    {
        context.Posts.Add(post);
        await context.SaveChangesAsync();

        return post;
    }

    public async Task<bool> UpdatePostAsync(Post post)
    {
        var result = await context.Posts
            .Where(x => x.Id == post.Id)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(dbPost => dbPost.Title, post.Title)
                .SetProperty(dbPost => dbPost.Content, post.Content)
                .SetProperty(dbPost => dbPost.TopicId, post.TopicId));


        return result > 0;
    }

    public async Task<bool> DeletePostAsync(long id)
    {
        var result = await context.Posts
            .Where(user => user.Id == id)
            .ExecuteDeleteAsync();

        return result > 0;
    }

    public async Task<Post> FindPostAsync(long id)
    {
        var post = await context.Posts.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id)
                   ?? throw new KeyNotFoundException("User not found.");

        return post;
    }

    public async Task<List<Post>> GetAllPostAsync()
    {
        return await context.Posts.AsNoTracking().ToListAsync();
    }
}
