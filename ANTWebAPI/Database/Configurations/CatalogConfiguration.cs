using ANTWebAPI.Database.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace ANTWebAPI.Database.Configurations;

internal class CatalogConfiguration : IEntityTypeConfiguration<Catalog>
{
    /// <summary>
    /// Configures the entity to match the database schema.
    /// </summary>
    /// <param name="entity">The entity to configure.</param>
    /// <remarks>
    /// Configures the entity to match the following database schema:
    /// <code>
    /// CREATE TABLE ANTArticles (
    ///     catalog_id INTEGER PRIMARY KEY,
    ///     name TEXT NOT NULL
    /// );
    /// </code>
    /// </remarks>
    public void Configure(EntityTypeBuilder<Catalog> entity)
    {
        // Configure the table name
        entity.ToTable("ANTCatalogs");

        // Configure the primary key
        entity.HasKey(t => t.Id);

        // Configure the properties
        entity.Property(t => t.Id)
            // Map the property to the 'catalog_id' column
            .HasColumnName("catalog_id")
            // Set the property as required
            .IsRequired();

        entity.Property(t => t.Name)
            // Map the property to the 'name' column
            .HasColumnName("name")
            // Set the property as required
            .IsRequired();

        // Configure the relationship with the Article entity
        entity.HasMany(i => i.Articles)
            // Set the navigation property on the Article entity
            .WithOne(i => i.Catalog)
            // Set the foreign key on the Article entity
            .HasForeignKey(i => i.CatalogId);
    }
}
