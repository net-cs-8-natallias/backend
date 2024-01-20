using Catalog.Host.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Host.Data.EntityConfiguration;

public class CatalogTypeEntityTypeConfiguration : IEntityTypeConfiguration<CatalogType>
{
    public void Configure(EntityTypeBuilder<CatalogType> builder)
    {
        builder.ToTable("catalogtype");
        builder.HasKey(catalogType => catalogType.Id);
        builder.Property(catalogType => catalogType.Id).HasColumnName("id")
            .IsRequired();

        builder.Property(catalogType => catalogType.Type).HasColumnName("type")
            .IsRequired()
            .HasMaxLength(50);
    }
}