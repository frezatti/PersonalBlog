using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalBlog.Models;

[Table("Topic")]
public class Topic
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    [Required]
    [StringLength(255)]
    [Column(TypeName = "varchar(255)")]
    public string Description { get; set; } = string.Empty;

    public ICollection<Post> Posts { get; set; } = new List<Post>();
}
