namespace PersonalBlog.DTOs.Post;

public class CreatePostDto
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public long TopicId { get; set; }
    public long UserId { get; set; }
}
