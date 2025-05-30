using Microsoft.EntityFrameworkCore;

namespace RainSeed.Tests.Storage;

public class TestDBContext : DbContext
{
    public DbSet<TokenEntity> Tokens { get; set; }
    public DbSet<DocumentTokenEntity> TokensDocuments { get; set; }

    public string DbPath { get; init; }

    public TestDBContext(string path = "test_index.db")
    {
        DbPath = path;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite($"Data Source={DbPath}");
    }
}
