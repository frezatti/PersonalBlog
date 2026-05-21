using Microsoft.EntityFrameworkCore;
using PersonalBlog.Data;
using PersonalBlog.Models;

namespace PersonalBlog.Services;

public class TopicService:ITopicService
{

    private readonly AppDBContext _context;
    TopicService(AppDBContext context)
    {
        _context = context;
    }

    public async Task<Topic> CreateTopicAsync(Topic topic)
    {
        await _context.Topics.AddAsync(topic);
        await _context.SaveChangesAsync();
        return topic;
    }

    public async Task<Topic> UpdateTopic(Topic topic)
    {
        var updatedTopic = await _context.Topics.Where(x => x.Id == topic.Id)
            .ExecuteUpdateAsync(setter =>
                setter.SetProperty(dbTopic => dbTopic.Description, topic.Description  ));
        return topic;
    }

    public async Task<int> DeleteTopic(long id)
    {
         var result = await _context.Topics.Where(x => x.Id == id).ExecuteDeleteAsync();
         return result;
    }
}