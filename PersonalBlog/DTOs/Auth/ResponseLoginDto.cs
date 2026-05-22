using PersonalBlog.Models.Enums;

namespace PersonalBlog.DTOs.Auth;

public class ResponseLoginDto
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public UserType Type { get; set; }
    public string Token { get; set; } = string.Empty;
}
