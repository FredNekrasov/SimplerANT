using System.Reflection;
using ANTWebAPI.Database.Configurations;
using ANTWebAPI.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace ANTWebAPI.Database;

public class ANTDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseSqlite("ANTDb.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly(), t => t.Name.EndsWith("Configuration"));
    }

    public DbSet<Catalog> Catalogs { get; set; }
    public DbSet<Article> Articles { get; set; }
    public DbSet<Content> Contents { get; set; }
}