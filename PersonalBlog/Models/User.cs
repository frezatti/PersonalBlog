using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

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
    public string Nome { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(255)]
    [Column(TypeName = "varchar(255)")]
    public string Email { get; set; } = string.Empty;

    [Required]
    [Column(TypeName = "text")]
    public string Senha { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    [Column(TypeName = "varchar(50)")]
    public string Tipo { get; set; } = "User";

    public ICollection<Post> Postagens { get; set; } = new List<Post>();
}
