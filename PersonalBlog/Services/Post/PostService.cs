using PersonalBlog.DTOs.Post;
using PersonalBlog.Models;
using PersonalBlog.Repositories;

namespace PersonalBlog.Services;

public class PostService(
    IPostRepository postRepository,
    IUserRepository userRepository,
    ITopicRepository topicRepository
) : IPostService
{
    public async Task<PostResponseDto> FindPostAsync(long id)
    {
        if (id <= 0)
            throw new ArgumentException("The post id is invalid.");

        var post = await postRepository.FindPostAsync(id);

        return ToResponseDto(post);
    }

    public async Task<List<PostResponseDto>> GetAllPostsAsync()
    {
        var posts = await postRepository.GetAllPostAsync();

        return posts.Select(ToResponseDto).ToList();
    }

    public async Task<List<PostResponseDto>> GetPostsAsync(long? userId, long? topicId)
    {
        if (userId is not null && userId <= 0)
            throw new ArgumentException("The user id is invalid.");

        if (topicId is not null && topicId <= 0)
            throw new ArgumentException("The topic id is invalid.");

        var posts = await postRepository.GetPostsAsync(userId, topicId);

        return posts.Select(ToResponseDto).ToList();
    }

    public async Task<PostResponseDto> CreatePostAsync(CreatePostDto postDto)
    {
        if (string.IsNullOrWhiteSpace(postDto.Title))
            throw new ArgumentException("The post title cannot be empty.");

        if (string.IsNullOrWhiteSpace(postDto.Content))
            throw new ArgumentException("The post content cannot be empty.");

        if (postDto.UserId <= 0)
            throw new ArgumentException("The user id is invalid.");

        if (postDto.TopicId <= 0)
            throw new ArgumentException("The topic id is invalid.");

        await userRepository.FindUserAsync(postDto.UserId);
        await topicRepository.FindTopicAsync(postDto.TopicId);

        var post = new Post
        {
            Title = postDto.Title.Trim(),
            Content = postDto.Content.Trim(),
            UserId = postDto.UserId,
            TopicId = postDto.TopicId
        };

        var createdPost = await postRepository.CreatePostAsync(post);

        var postWithRelations = await postRepository.FindPostAsync(createdPost.Id);

        return ToResponseDto(postWithRelations);
    }

    public async Task<PostResponseDto> UpdatePostAsync(UpdatePostDto postDto)
    {
        if (postDto.Id <= 0)
            throw new ArgumentException("The post id is invalid.");

        if (string.IsNullOrWhiteSpace(postDto.Title))
            throw new ArgumentException("The post title cannot be empty.");

        if (string.IsNullOrWhiteSpace(postDto.Content))
            throw new ArgumentException("The post content cannot be empty.");

        if (postDto.TopicId <= 0)
            throw new ArgumentException("The topic id is invalid.");

        await topicRepository.FindTopicAsync(postDto.TopicId);

        var post = new Post
        {
            Id = postDto.Id,
            Title = postDto.Title.Trim(),
            Content = postDto.Content.Trim(),
            TopicId = postDto.TopicId
        };

        var updated = await postRepository.UpdatePostAsync(post);

        if (!updated)
            throw new KeyNotFoundException("Post not found.");

        var updatedPost = await postRepository.FindPostAsync(postDto.Id);

        return ToResponseDto(updatedPost);
    }

    public async Task DeletePostAsync(long id)
    {
        if (id <= 0)
            throw new ArgumentException("The post id is invalid.");

        var deleted = await postRepository.DeletePostAsync(id);

        if (!deleted)
            throw new KeyNotFoundException("Post not found.");
    }

    private static PostResponseDto ToResponseDto(Post post)
    {
        return new PostResponseDto
        {
            Id = post.Id,
            Title = post.Title,
            Content = post.Content,
            CreatedAt = post.CreatedAt,
            UserId = post.UserId,
            UserName = post.User?.Name,
            TopicId = post.TopicId,
            TopicDescription = post.Topic?.Description,
            AiSummary = post.AiSummary,
            AiTags = post.AiTags,
            AiCategory = post.AiCategory
        };
    }
}
