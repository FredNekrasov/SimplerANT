using System.Reflection;
using ANTWebAPI.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace ANTWebAPI.Database;

public class ANTDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseSqlite("Data Source = ANTDb.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // Applies configurations for all entities from the current assembly.
        // This allows automatic loading of configurations defined in classes that implement IEntityTypeConfiguration.
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public DbSet<Catalog> Catalogs { get; set; }
    public DbSet<Article> Articles { get; set; }
    public DbSet<Content> Contents { get; set; }
}