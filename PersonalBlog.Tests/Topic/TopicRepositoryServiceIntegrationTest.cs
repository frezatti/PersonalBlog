using Microsoft.EntityFrameworkCore;
using PersonalBlog.Data;
using PersonalBlog.DTOs.Topic;
using PersonalBlog.Repositories;
using PersonalBlog.Services;

namespace PersonalBlog.Tests;

public class TopicServiceIntegrationTests
{
    private static AppDBContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDBContext>()
            .UseNpgsql("Host=localhost;Port=5432;Database=personal_blog_test_db;Username=postgres;Password=password")
            .Options;

        var context = new AppDBContext(options);

        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        return context;
    }

    [Fact]
    public async Task CreateTopicAsync_WithRealRepository_SavesTopicInDatabase()
    {
        await using var context = CreateContext();

        var repository = new TopicRepository(context);
        var service = new TopicService(repository);

        var result = await service.CreateTopicAsync(new CreateTopicDto
        {
            Description = " Technology "
        });

        Assert.True(result.Id > 0);
        Assert.Equal("Technology", result.Description);

        var topicFromDatabase = await context.Topics.FindAsync(result.Id);

        Assert.NotNull(topicFromDatabase);
        Assert.Equal("Technology", topicFromDatabase.Description);
    }
}
