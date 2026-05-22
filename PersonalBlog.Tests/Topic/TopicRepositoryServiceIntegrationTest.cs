using Microsoft.EntityFrameworkCore;
using PersonalBlog.DTOs.Topic;
using PersonalBlog.Repositories;
using PersonalBlog.Services;

namespace PersonalBlog.Tests;

public class TopicServiceIntegrationTests
{
    public TopicServiceIntegrationTests()
    {
        TestDatabase.Reset();
    }

    [Fact]
    public async Task CreateTopicAsync_WithRealRepository_SavesTopicInDatabase()
    {
        await using var context = TestDatabase.CreateContext();

        var repository = new TopicRepository(context);
        var service = new TopicService(repository);

        var result = await service.CreateTopicAsync(new CreateTopicDto
        {
            Description = " Technology "
        });

        Assert.True(result.Id > 0);
        Assert.Equal("Technology", result.Description);

        await using var verifyContext = TestDatabase.CreateContext();

        var topicFromDatabase = await verifyContext.Topics
            .AsNoTracking()
            .FirstOrDefaultAsync(dbTopic => dbTopic.Id == result.Id);

        Assert.NotNull(topicFromDatabase);
        Assert.Equal("Technology", topicFromDatabase.Description);
    }
}
