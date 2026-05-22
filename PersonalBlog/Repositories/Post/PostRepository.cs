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
            .Where(dbPost => dbPost.Id == post.Id)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(dbPost => dbPost.Title, post.Title)
                .SetProperty(dbPost => dbPost.Content, post.Content)
                .SetProperty(dbPost => dbPost.TopicId, post.TopicId));

        return result > 0;
    }

    public async Task<bool> DeletePostAsync(long id)
    {
        var result = await context.Posts
            .Where(post => post.Id == id)
            .ExecuteDeleteAsync();

        return result > 0;
    }

    public async Task<Post> FindPostAsync(long id)
    {
        var post = await context.Posts
            .AsNoTracking()
            .Include(post => post.User)
            .Include(post => post.Topic)
            .FirstOrDefaultAsync(post => post.Id == id)
            ?? throw new KeyNotFoundException("Post not found.");

        return post;
    }

    public async Task<List<Post>> GetPostsAsync(long? userId, long? topicId)
    {
        var query = context.Posts
            .AsNoTracking()
            .Include(post => post.User)
            .Include(post => post.Topic)
            .AsQueryable();

        if (userId is not null)
            query = query.Where(post => post.UserId == userId);

        if (topicId is not null)
            query = query.Where(post => post.TopicId == topicId);

        return await query
            .OrderByDescending(post => post.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<Post>> GetAllPostAsync()
    {
        return await GetPostsAsync(null, null);
    }
}
