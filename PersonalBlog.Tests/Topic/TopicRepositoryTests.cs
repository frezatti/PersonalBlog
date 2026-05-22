using Microsoft.EntityFrameworkCore;
using PersonalBlog.Data;
using PersonalBlog.Models;
using PersonalBlog.Repositories;

namespace PersonalBlog.Tests;

public class TopicRepositoryTests
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
    public async Task CreateTopicAsync_SavesTopicInDatabase()
    {
        await using var context = CreateContext();

        var repository = new TopicRepository(context);

        var topic = new Topic
        {
            Description = "Technology"
        };

        var result = await repository.CreateTopicAsync(topic);

        Assert.True(result.Id > 0);

        var topicFromDatabase = await context.Topics.FindAsync(result.Id);

        Assert.NotNull(topicFromDatabase);
        Assert.Equal("Technology", topicFromDatabase.Description);
    }

    [Fact]
    public async Task UpdateTopicAsync_UpdatesTopicInDatabase()
    {
        await using var context = CreateContext();

        context.Topics.Add(new Topic { Description = "Old description" });
        await context.SaveChangesAsync();

        var topic = await context.Topics.FirstAsync();

        var repository = new TopicRepository(context);

        var updated = await repository.UpdateTopicAsync(new Topic
        {
            Id = topic.Id,
            Description = "New description"
        });

        Assert.True(updated);

        var topicFromDatabase = await context.Topics.FindAsync(topic.Id);

        Assert.NotNull(topicFromDatabase);
        Assert.Equal("New description", topicFromDatabase.Description);
    }

    [Fact]
    public async Task DeleteTopicAsync_DeletesTopicFromDatabase()
    {
        await using var context = CreateContext();

        context.Topics.Add(new Topic { Description = "Technology" });
        await context.SaveChangesAsync();

        var topic = await context.Topics.FirstAsync();

        var repository = new TopicRepository(context);

        var deleted = await repository.DeleteTopicAsync(topic.Id);

        Assert.True(deleted);

        var topicFromDatabase = await context.Topics.FindAsync(topic.Id);

        Assert.Null(topicFromDatabase);
    }

    [Fact]
    public async Task FindTopicAsync_WhenTopicDoesNotExist_ThrowsKeyNotFoundException()
    {
        await using var context = CreateContext();

        var repository = new TopicRepository(context);

        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            repository.FindTopicAsync(999));
    }
}
