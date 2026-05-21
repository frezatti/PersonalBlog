using Microsoft.EntityFrameworkCore;
using PersonalBlog.Models;

namespace PersonalBlog.Data;

public class AppDBContext : DbContext
{
    public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
    { }

    public DbSet<Post> Posts => Set<Post>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Topic> Topics => Set<Topic>();

}
