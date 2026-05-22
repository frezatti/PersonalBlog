using PersonalBlog.DTOs.Topic;
using PersonalBlog.Models;
using PersonalBlog.Repositories;

namespace PersonalBlog.Services;

public class TopicService(ITopicRepository topicRepository) : ITopicService
{
    public async Task<ResponseTopicDto> FindTopicAsync(long id)
    {
        var topic = await topicRepository.FindTopicAsync(id);

        return ToResponseDto(topic);
    }

    public async Task<List<ResponseTopicDto>> GetAllTopicsAsync()
    {
        var topics = await topicRepository.GetAllTopicsAsync();

        return topics.Select(ToResponseDto).ToList();
    }

    public async Task<ResponseTopicDto> CreateTopicAsync(CreateTopicDto topicDto)
    {
        if (string.IsNullOrWhiteSpace(topicDto.Description))
            throw new ArgumentException("The topic description cannot be empty.");

        var topic = new Topic
        {
            Description = topicDto.Description.Trim()
        };

        var createdTopic = await topicRepository.CreateTopicAsync(topic);

        return ToResponseDto(createdTopic);
    }

    public async Task<ResponseTopicDto> UpdateTopicAsync(UpdateTopicDto topicDto)
    {
        if (topicDto.Id <= 0)
            throw new ArgumentException("The topic id cannot be 0.");

        if (string.IsNullOrWhiteSpace(topicDto.Description))
            throw new ArgumentException("The topic description cannot be empty.");

        var updatedTopic = new Topic
        {
            Id = topicDto.Id,
            Description = topicDto.Description.Trim()
        };

        var updated = await topicRepository.UpdateTopicAsync(updatedTopic);

        if (!updated)
            throw new KeyNotFoundException("Topic not found.");

        return ToResponseDto(updatedTopic);
    }

    public async Task DeleteTopicAsync(long id)
    {
        var deleted = await topicRepository.DeleteTopicAsync(id);
        
        if (!deleted) throw new KeyNotFoundException("Topic not found.");
    }


    private static ResponseTopicDto ToResponseDto(Topic topic)
    {
        return new ResponseTopicDto
        {
            Id = topic.Id,
            Description = topic.Description
        };
    }
}
