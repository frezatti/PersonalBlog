using Microsoft.EntityFrameworkCore;
using PersonalBlog.Models;
using PersonalBlog.Repositories;

namespace PersonalBlog.Tests;

public class TopicRepositoryTests
{
    public TopicRepositoryTests()
    {
        TestDatabase.Reset();
    }

    [Fact]
    public async Task CreateTopicAsync_SavesTopicInDatabase()
    {
        await using var context = TestDatabase.CreateContext();

        var repository = new TopicRepository(context);

        var topic = new Topic
        {
            Description = "Technology"
        };

        var result = await repository.CreateTopicAsync(topic);

        Assert.True(result.Id > 0);

        await using var verifyContext = TestDatabase.CreateContext();

        var topicFromDatabase = await verifyContext.Topics
            .AsNoTracking()
            .FirstOrDefaultAsync(dbTopic => dbTopic.Id == result.Id);

        Assert.NotNull(topicFromDatabase);
        Assert.Equal("Technology", topicFromDatabase.Description);
    }

    [Fact]
    public async Task UpdateTopicAsync_UpdatesTopicInDatabase()
    {
        long topicId;

        await using (var arrangeContext = TestDatabase.CreateContext())
        {
            arrangeContext.Topics.Add(new Topic { Description = "Old description" });
            await arrangeContext.SaveChangesAsync();

            topicId = await arrangeContext.Topics
                .Select(topic => topic.Id)
                .FirstAsync();
        }

        await using (var actContext = TestDatabase.CreateContext())
        {
            var repository = new TopicRepository(actContext);

            var updated = await repository.UpdateTopicAsync(new Topic
            {
                Id = topicId,
                Description = "New description"
            });

            Assert.True(updated);
        }

        await using (var verifyContext = TestDatabase.CreateContext())
        {
            var topicFromDatabase = await verifyContext.Topics
                .AsNoTracking()
                .FirstOrDefaultAsync(dbTopic => dbTopic.Id == topicId);

            Assert.NotNull(topicFromDatabase);
            Assert.Equal("New description", topicFromDatabase.Description);
        }
    }

    [Fact]
    public async Task DeleteTopicAsync_DeletesTopicFromDatabase()
    {
        long topicId;

        await using (var arrangeContext = TestDatabase.CreateContext())
        {
            arrangeContext.Topics.Add(new Topic { Description = "Technology" });
            await arrangeContext.SaveChangesAsync();

            topicId = await arrangeContext.Topics
                .Select(topic => topic.Id)
                .FirstAsync();
        }

        await using (var actContext = TestDatabase.CreateContext())
        {
            var repository = new TopicRepository(actContext);

            var deleted = await repository.DeleteTopicAsync(topicId);

            Assert.True(deleted);
        }

        await using (var verifyContext = TestDatabase.CreateContext())
        {
            var topicFromDatabase = await verifyContext.Topics
                .AsNoTracking()
                .FirstOrDefaultAsync(dbTopic => dbTopic.Id == topicId);

            Assert.Null(topicFromDatabase);
        }
    }

    [Fact]
    public async Task FindTopicAsync_WhenTopicDoesNotExist_ThrowsKeyNotFoundException()
    {
        await using var context = TestDatabase.CreateContext();

        var repository = new TopicRepository(context);

        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            repository.FindTopicAsync(999));
    }
}
