using Microsoft.EntityFrameworkCore;
using PersonalBlog.Data;
using PersonalBlog.Models;

namespace PersonalBlog.Repositories;

public class TopicRepository : ITopicRepository
{
    private readonly AppDBContext _context;

    public TopicRepository(AppDBContext context)
    {
        _context = context;
    }

    public async Task<Topic> CreateTopicAsync(Topic topic)
    {
        _context.Topics.Add(topic);
        await _context.SaveChangesAsync();

        return topic;
    }

    public async Task<bool> UpdateTopicAsync(Topic topic)
    {
        var result = await _context.Topics
            .Where(dbTopic => dbTopic.Id == topic.Id)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(dbTopic => dbTopic.Description, topic.Description));

        return result > 0;
    }

    public async Task<bool> DeleteTopicAsync(long id)
    {
        var result = await _context.Topics
            .Where(topic => topic.Id == id)
            .ExecuteDeleteAsync();

        return result > 0;
    }

    public async Task<Topic> FindTopicAsync(long id)
    {
        var topic = await _context.Topics.FindAsync(id)
            ?? throw new KeyNotFoundException("Topic not found.");

        return topic;
    }

    public async Task<List<Topic>> GetAllTopicsAsync()
    {
        return await _context.Topics.ToListAsync();
    }
}
