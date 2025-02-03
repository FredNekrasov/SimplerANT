using ANTWebAPI.Database.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace ANTWebAPI.Database.Configurations;

internal class ArticleConfiguration : IEntityTypeConfiguration<Article>
{
    public void Configure(EntityTypeBuilder<Article> entity)
    {
        entity.ToTable("ANTArticles")
            .HasKey(t => t.Id);

        entity.Property(t => t.Id)
            .HasColumnName("article_id")
            .IsRequired();
        
        entity.Property(t => t.CatalogId)
            .HasColumnName("catalog_id")
            .IsRequired();
        
        entity.Property(t => t.Title)
            .HasColumnName("title")
            .IsRequired();
        
        entity.Property(t => t.Description)
            .HasColumnName("description")
            .IsRequired();
        
        entity.Property(t => t.DateOrBanner)
            .HasColumnName("date_or_banner")
            .IsRequired();

        entity.HasMany(t => t.Contents)
            .WithOne(t => t.Article)
            .HasForeignKey(t => t.ArticleId);
    }
}
