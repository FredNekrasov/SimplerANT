using System.Reflection;
using ANTWebAPI.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace ANTWebAPI.Database;

/// <summary>
/// This class represents the database context, which is the main entry point to the Entity Framework Core API.
/// </summary>
/// <remarks>
/// This class is responsible for configuration of the database connection, mapping of entities to database tables
/// and defining the database schema.
/// </remarks>
public class ANTDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        // Use SQLite database file named ANTDb.db.
        optionsBuilder.UseSqlite("Data Source = ANTDb.db");
    }

    /// <summary>
    /// Configures the model by applying entity configurations from the current assembly.
    /// </summary>
    /// <param name="modelBuilder">The <see cref="ModelBuilder"/> to configure the model.</param>
    /// <remarks>
    /// This method is called when the model is being configured. It is responsible for applying any
    /// entity configurations defined in the current assembly. This allows automatic loading of
    /// configurations defined in classes that implement <see cref="IEntityTypeConfiguration{TEntity}"/>.
    /// </remarks>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public DbSet<Catalog> Catalogs { get; set; }
    public DbSet<Article> Articles { get; set; }
    public DbSet<Content> Contents { get; set; }
}
