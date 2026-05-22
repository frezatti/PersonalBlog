using PersonalBlog.DTOs.Post;

namespace PersonalBlog.Services;

public interface IPostService
{
    Task<PostResponseDto> CreatePostAsync(CreatePostDto postDto);
    Task<PostResponseDto> UpdatePostAsync(UpdatePostDto postDto);
    Task DeletePostAsync(long id);
    Task<PostResponseDto> FindPostAsync(long id);
    Task<List<PostResponseDto>> GetAllPostsAsync();
}
