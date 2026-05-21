using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
namespace PersonalBlog.Models;

public class UserLogin
{

    [ForeignKey(nameof(User))]
    public long UserId { get; set; }
    public string PassowordHash { get; set; } = string.Empty;
}
