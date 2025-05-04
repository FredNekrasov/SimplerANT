using ANTWebAPI.Database.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace ANTWebAPI.Database.Configurations;

internal class ContentConfiguration : IEntityTypeConfiguration<Content>
{
    /// <summary>
    /// Configures the entity to match the database schema.
    /// </summary>
    /// <param name="entity">The entity to configure.</param>
    /// <remarks>
    /// Configures the entity to match the following database schema:
    /// <code>
    /// CREATE TABLE ANTContents (
    ///     content_id INTEGER PRIMARY KEY,
    ///     article_id INTEGER NOT NULL,
    ///     data TEXT NOT NULL
    /// );
    /// </code>
    /// </remarks>
    public void Configure(EntityTypeBuilder<Content> entity)
    {
        // Configure the table name
        entity.ToTable("ANTContents")
            // Configure the primary key
            .HasKey(t => t.Id);

        // Configure the properties
        entity.Property(t => t.Id)
            // Map the property to the 'content_id' column
            .HasColumnName("content_id")
            // Set the property as required
            .IsRequired();

        entity.Property(t => t.ArticleId)
            // Map the property to the 'article_id' column
            .HasColumnName("article_id")
            // Set the property as required
            .IsRequired();

        entity.Property(t => t.Data)
            // Map the property to the 'data' column
            .HasColumnName("data")
            // Set the property as required
            .IsRequired();
    }
}
