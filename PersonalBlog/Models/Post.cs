using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalBlog.Models;

[Table("Post")]
public class Post
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    [Required]
    [StringLength(100)]
    [Column(TypeName = "varchar(100)")]
    public string Titulo { get; set; } = string.Empty;

    [Required]
    [Column(TypeName = "text")]
    public string Texto { get; set; } = string.Empty;

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime Data { get; set; } = DateTime.UtcNow;

    [ForeignKey(nameof(User))]
    public long UserId { get; set; }

    public User? User { get; set; }

    [ForeignKey(nameof(Topic))]
    public long TopicId { get; set; }

    public Topic? Topic { get; set; }

    [Column(TypeName = "text")]
    public string? ResumoIA { get; set; }

    [Column(TypeName = "text")]
    public string? TagsIA { get; set; }

    [Column(TypeName = "varchar(255)")]
    public string? CatagoryIA { get; set; }
}
