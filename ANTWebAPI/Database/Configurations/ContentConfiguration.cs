using ANTWebAPI.Database.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace ANTWebAPI.Database.Configurations;

internal class ContentConfiguration : IEntityTypeConfiguration<Content>
{
    public void Configure(EntityTypeBuilder<Content> entity)
    {
        entity.ToTable("ANTContents")
            .HasKey(t => t.Id);
        
        entity.Property(t => t.Id)
            .HasColumnName("content_id")
            .IsRequired();
        
        entity.Property(t => t.ArticleId)
            .HasColumnName("article_id")
            .IsRequired();
        
        entity.Property(t => t.Data)
            .HasColumnName("data")
            .IsRequired();
    }
}
