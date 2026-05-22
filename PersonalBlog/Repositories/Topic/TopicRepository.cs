using Microsoft.EntityFrameworkCore;
using PersonalBlog.Data;
using PersonalBlog.Models;

namespace PersonalBlog.Repositories;

public class TopicRepository(AppDBContext context) : ITopicRepository
{
    public async Task<Topic> CreateTopicAsync(Topic topic)
    {
        context.Topics.Add(topic);
        await context.SaveChangesAsync();

        return topic;
    }

    public async Task<bool> UpdateTopicAsync(Topic topic)
    {
        var result = await context.Topics
            .Where(dbTopic => dbTopic.Id == topic.Id)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(dbTopic => dbTopic.Description, topic.Description));

        return result > 0;
    }

    public async Task<bool> DeleteTopicAsync(long id)
    {
        var result = await context.Topics
            .Where(topic => topic.Id == id)
            .ExecuteDeleteAsync();

        return result > 0;
    }

    public async Task<Topic> FindTopicAsync(long id)
    {
        var topic = await context.Topics.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new KeyNotFoundException("Topic not found.");

        return topic;
    }

    public async Task<List<Topic>> GetAllTopicsAsync()
    {
        return await context.Topics.AsNoTracking().ToListAsync();
    }
}
