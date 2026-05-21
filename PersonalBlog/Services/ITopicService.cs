using PersonalBlog.DTOs.Topic;

namespace PersonalBlog.Services;


public interface ITopicService
{
    Task<ResponseTopicDto> CreateTopicAsync(CreateTopicDto topicDto);
    Task<ResponseTopicDto> UpdateTopicAsync(UpdateTopicDto topicDto);
    Task DeleteTopicAsync(long id);
    Task<ResponseTopicDto> FindTopicAsync(long id);
    Task<List<ResponseTopicDto>> GetAllTopicsAsync();
}
