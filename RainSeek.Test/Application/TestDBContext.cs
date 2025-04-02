using System.IO;
using Microsoft.EntityFrameworkCore;

namespace RainSeed.Tests.Application;

public class TestDBContext : DbContext
{
    public DbSet<TokenEntity> Tokens { get; set; }
    public DbSet<TokensDocumentsEntity> TokensDocuments { get; set; }

    public string DbPath { get; init; }

    public TestDBContext(string path = "test_index.db")
    {
        DbPath = path;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");
}
