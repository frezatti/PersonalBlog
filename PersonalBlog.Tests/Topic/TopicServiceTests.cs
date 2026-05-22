using PersonalBlog.DTOs.Topic;
using PersonalBlog.Models;
using PersonalBlog.Repositories;
using PersonalBlog.Services;

namespace PersonalBlog.Tests;

public class TopicServiceTests
{
    [Fact]
    public async Task CreateTopicAsync_WithValidDto_ReturnsResponseTopicDto()
    {
        var repository = new FakeTopicRepository();
        var service = new TopicService(repository);

        var dto = new CreateTopicDto
        {
            Description = " Technology "
        };

        var result = await service.CreateTopicAsync(dto);

        Assert.Equal(1, result.Id);
        Assert.Equal("Technology", result.Description);
    }

    [Fact]
    public async Task CreateTopicAsync_WithEmptyDescription_ThrowsArgumentException()
    {
        var repository = new FakeTopicRepository();
        var service = new TopicService(repository);

        var dto = new CreateTopicDto
        {
            Description = ""
        };

        await Assert.ThrowsAsync<ArgumentException>(() =>
            service.CreateTopicAsync(dto));
    }

    [Fact]
    public async Task CreateTopicAsync_WithOnlySpacesDescription_ThrowsArgumentException()
    {
        var repository = new FakeTopicRepository();
        var service = new TopicService(repository);

        var dto = new CreateTopicDto
        {
            Description = "   "
        };

        await Assert.ThrowsAsync<ArgumentException>(() =>
            service.CreateTopicAsync(dto));
    }

    [Fact]
    public async Task UpdateTopicAsync_WithValidDto_ReturnsUpdatedTopic()
    {
        var repository = new FakeTopicRepository([
            new Topic { Id = 1, Description = "Old description" }
        ]);

        var service = new TopicService(repository);

        var dto = new UpdateTopicDto
        {
            Id = 1,
            Description = "New description"
        };

        var result = await service.UpdateTopicAsync(dto);

        Assert.Equal(1, result.Id);
        Assert.Equal("New description", result.Description);
    }

    [Fact]
    public async Task UpdateTopicAsync_WithIdZero_ThrowsArgumentException()
    {
        var repository = new FakeTopicRepository();
        var service = new TopicService(repository);

        var dto = new UpdateTopicDto
        {
            Id = 0,
            Description = "Valid description"
        };

        await Assert.ThrowsAsync<ArgumentException>(() =>
            service.UpdateTopicAsync(dto));
    }

    [Fact]
    public async Task UpdateTopicAsync_WithEmptyDescription_ThrowsArgumentException()
    {
        var repository = new FakeTopicRepository();
        var service = new TopicService(repository);

        var dto = new UpdateTopicDto
        {
            Id = 1,
            Description = ""
        };

        await Assert.ThrowsAsync<ArgumentException>(() =>
            service.UpdateTopicAsync(dto));
    }

    [Fact]
    public async Task UpdateTopicAsync_WhenTopicDoesNotExist_ThrowsKeyNotFoundException()
    {
        var repository = new FakeTopicRepository();
        var service = new TopicService(repository);

        var dto = new UpdateTopicDto
        {
            Id = 999,
            Description = "New description"
        };

        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            service.UpdateTopicAsync(dto));
    }

    [Fact]
    public async Task DeleteTopicAsync_WhenTopicExists_DoesNotThrowException()
    {
        var repository = new FakeTopicRepository([
            new Topic { Id = 1, Description = "Technology" }
        ]);

        var service = new TopicService(repository);

        await service.DeleteTopicAsync(1);
    }

    [Fact]
    public async Task DeleteTopicAsync_WhenTopicDoesNotExist_ThrowsKeyNotFoundException()
    {
        var repository = new FakeTopicRepository();
        var service = new TopicService(repository);

        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            service.DeleteTopicAsync(999));
    }

    [Fact]
    public async Task FindTopicAsync_WhenTopicExists_ReturnsTopic()
    {
        var repository = new FakeTopicRepository([
            new Topic { Id = 1, Description = "Technology" }
        ]);

        var service = new TopicService(repository);

        var result = await service.FindTopicAsync(1);

        Assert.Equal(1, result.Id);
        Assert.Equal("Technology", result.Description);
    }

    [Fact]
    public async Task FindTopicAsync_WhenTopicDoesNotExist_ThrowsKeyNotFoundException()
    {
        var repository = new FakeTopicRepository();
        var service = new TopicService(repository);

        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            service.FindTopicAsync(999));
    }

    [Fact]
    public async Task GetAllTopicsAsync_ReturnsAllTopics()
    {
        var repository = new FakeTopicRepository([
            new Topic { Id = 1, Description = "Technology" },
            new Topic { Id = 2, Description = "Games" }
        ]);

        var service = new TopicService(repository);

        var result = await service.GetAllTopicsAsync();

        Assert.Equal(2, result.Count);
        Assert.Equal("Technology", result[0].Description);
        Assert.Equal("Games", result[1].Description);
    }

    private sealed class FakeTopicRepository : ITopicRepository
    {
        private readonly List<Topic> _topics = [];
        private long _nextId = 1;

        public FakeTopicRepository(IEnumerable<Topic>? topics = null)
        {
            if (topics is null)
                return;

            _topics.AddRange(topics);

            if (_topics.Count > 0)
                _nextId = _topics.Max(topic => topic.Id) + 1;
        }

        public Task<Topic> CreateTopicAsync(Topic topic)
        {
            topic.Id = _nextId++;
            _topics.Add(topic);

            return Task.FromResult(topic);
        }

        public Task<bool> UpdateTopicAsync(Topic topic)
        {
            var existingTopic = _topics.FirstOrDefault(dbTopic => dbTopic.Id == topic.Id);

            if (existingTopic is null)
                return Task.FromResult(false);

            existingTopic.Description = topic.Description;

            return Task.FromResult(true);
        }

        public Task<bool> DeleteTopicAsync(long id)
        {
            var existingTopic = _topics.FirstOrDefault(topic => topic.Id == id);

            if (existingTopic is null)
                return Task.FromResult(false);

            _topics.Remove(existingTopic);

            return Task.FromResult(true);
        }

        public Task<Topic> FindTopicAsync(long id)
        {
            var topic = _topics.FirstOrDefault(topic => topic.Id == id)
                ?? throw new KeyNotFoundException("Topic not found.");

            return Task.FromResult(topic);
        }

        public Task<List<Topic>> GetAllTopicsAsync()
        {
            return Task.FromResult(_topics.ToList());
        }
    }
}
