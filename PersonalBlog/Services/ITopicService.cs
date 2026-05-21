using PersonalBlog.Models;

namespace PersonalBlog.Services;

public interface ITopicService
{
    Task<Topic> CreateTopicAsync(Topic topic);
    Task<Topic> UpdateTopic(Topic topic);
    Task<int> DeleteTopic(long id);
}