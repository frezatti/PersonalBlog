namespace PersonalBlog.DTOs.Post;

public class UpdatePostDto
{
    public long Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public long TopicId { get; set; }
}
