using Microsoft.EntityFrameworkCore;
using PersonalBlog.Data;

namespace PersonalBlog.Tests;

public static class TestDatabase
{
    private const string ConnectionString =
        "Host=localhost;Port=5432;Database=personal_blog_test_db;Username=postgres;Password=password";

    private static readonly object Lock = new();
    private static bool _migrated;

    public static AppDBContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDBContext>()
            .UseNpgsql(ConnectionString)
            .Options;

        return new AppDBContext(options);
    }

    public static void Reset()
    {
        lock (Lock)
        {
            using var context = CreateContext();

            if (!_migrated)
            {
                context.Database.Migrate();
                _migrated = true;
            }

            context.Database.ExecuteSqlRaw("""
                TRUNCATE TABLE "Post", "Topic", "User" RESTART IDENTITY CASCADE;
            """);
        }
    }
}
