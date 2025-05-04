using ANTWebAPI.Database.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace ANTWebAPI.Database.Configurations;

internal class ArticleConfiguration : IEntityTypeConfiguration<Article>
{
    /// <summary>
    /// Configures the entity to match the database schema.
    /// </summary>
    /// <param name="entity">The entity to configure.</param>
    /// <remarks>
    /// Configures the entity to match the following database schema:
    /// <code>
    /// CREATE TABLE ANTArticles (
    ///     article_id INTEGER PRIMARY KEY,
    ///     catalog_id INTEGER NOT NULL,
    ///     title TEXT NOT NULL,
    ///     description TEXT NOT NULL,
    ///     date_or_banner TEXT NOT NULL
    /// );
    /// </code>
    /// </remarks>
    public void Configure(EntityTypeBuilder<Article> entity)
    {
        // Configure the table name
        entity.ToTable("ANTArticles")
            // Configure the primary key
            .HasKey(t => t.Id);

        // Configure the properties
        entity.Property(t => t.Id)
            // Map the property to the 'article_id' column
            .HasColumnName("article_id")
            // Set the property as required
            .IsRequired();
        
        entity.Property(t => t.CatalogId)
            // Map the property to the 'catalog_id' column
            .HasColumnName("catalog_id")
            // Set the property as required
            .IsRequired();
        
        entity.Property(t => t.Title)
            // Map the property to the 'title' column
            .HasColumnName("title")
            // Set the property as required
            .IsRequired();
        
        entity.Property(t => t.Description)
            // Map the property to the 'description' column
            .HasColumnName("description")
            // Set the property as required
            .IsRequired();
        
        entity.Property(t => t.DateOrBanner)
            // Map the property to the 'date_or_banner' column
            .HasColumnName("date_or_banner")
            // Set the property as required
            .IsRequired();

        // Configure the relationship with the Content entity
        entity.HasMany(t => t.Contents)
            // Set the navigation property on the Content entity
            .WithOne(t => t.Article)
            // Set the foreign key on the Content entity
            .HasForeignKey(t => t.ArticleId);
    }
}
