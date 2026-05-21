using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using PersonalBlog.Models.Enums;

namespace PersonalBlog.Models;

[Index(nameof(Email), IsUnique = true)]
[Table("User")]
public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    [Required]
    [StringLength(255)]
    [Column(TypeName = "varchar(255)")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(255)]
    [Column(TypeName = "varchar(255)")]
    public string Email { get; set; } = string.Empty;

    [Required]
    [Column(TypeName = "text")]
    public string Password { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    [Column(TypeName = "varchar(50)")]
    public UserType Type { get; set; } = UserType.User;

    public ICollection<Post> Posts { get; set; } = new List<Post>();

}
