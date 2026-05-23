using System.ComponentModel.DataAnnotations;

namespace PersonalBlog.DTOs.User;

public class UpdateUserDto
{
    public long Id { get; set; }

    [Required(ErrorMessage = "The user name is required.")]
    [StringLength(255, MinimumLength = 2, ErrorMessage = "The user name must be between 2 and 255 characters.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "The user email is required.")]
    [EmailAddress(ErrorMessage = "The email format is invalid.")]
    [StringLength(255, ErrorMessage = "The email must have at most 255 characters.")]
    public string Email { get; set; } = string.Empty;
}
