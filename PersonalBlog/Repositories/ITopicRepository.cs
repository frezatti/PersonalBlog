using PersonalBlog.Models;

namespace PersonalBlog.Repositories;

public interface ITopicRepository
{
    Task<Topic> CreateTopicAsync(Topic topic);
    Task<bool> UpdateTopicAsync(Topic topic);
    Task<bool> DeleteTopicAsync(long id);
    Task<Topic> FindTopicAsync(long id);
    Task<List<Topic>> GetAllTopicsAsync();
}
