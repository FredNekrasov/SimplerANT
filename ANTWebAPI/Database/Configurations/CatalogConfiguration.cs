using ANTWebAPI.Database.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace ANTWebAPI.Database.Configurations;

internal class CatalogConfiguration : IEntityTypeConfiguration<Catalog>
{
    public void Configure(EntityTypeBuilder<Catalog> entity)
    {
        entity.ToTable("ANTCatalogs")
            .HasKey(t => t.Id);

        entity.Property(t => t.Id)
            .HasColumnName("catalog_id")
            .IsRequired();
        entity.Property(t => t.Name)
            .HasColumnName("name")
            .IsRequired();
        
        entity.HasMany(i => i.Articles)
            .WithOne(i => i.Catalog)
            .HasForeignKey(i => i.CatalogId);
    }
}
