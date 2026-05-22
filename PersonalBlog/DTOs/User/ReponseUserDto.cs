using PersonalBlog.Models.Enums;

namespace PersonalBlog.DTOs.User;


public class ResponseUserDto
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public UserType Type { get; set; }
}
