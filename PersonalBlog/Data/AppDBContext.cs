using Microsoft.EntityFrameworkCore;
using PersonalBlog.Models;

namespace PersonalBlog.Data;

public class AppDBContext : DbContext
{
    public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
    { }

    public DbSet<Post> Post => Set<Post>();
    public DbSet<User> User => Set<User>();
    public DbSet<Topic> Topic => Set<Topic>();

}
