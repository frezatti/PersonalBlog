using PersonalBlog.Models.Enums;

namespace PersonalBlog.DTOs.Auth;

public class ReposeLoginDto
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    // public string HasedPassword { get; set; } = string.Empty;
    public UserType Type { get; set; }
}
