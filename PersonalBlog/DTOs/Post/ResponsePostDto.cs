namespace PersonalBlog.DTOs.Post;

public class PostResponseDto
{
    public long Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

    public long UserId { get; set; }
    public string? UserName { get; set; }

    public long TopicId { get; set; }
    public string? TopicDescription { get; set; }

    public string? AiSummary { get; set; }
    public string? AiTags { get; set; }
    public string? AiCategory { get; set; }
}
