using Catalog.Host.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Host.Data.EntityConfiguration;

public class CatalogBrandEntityTypeConfiguration : IEntityTypeConfiguration<CatalogBrand>
{
    public void Configure(EntityTypeBuilder<CatalogBrand> builder)
    {
        builder.ToTable("CatalogBrand");
        builder.HasKey(catalogBrand => catalogBrand.Id);
        builder.Property(catalogBrand => catalogBrand.Id)
            .UseHiLo("catalog_brand_hilo")
            .IsRequired();

        builder.Property(catalogBrand => catalogBrand.Brand)
            .IsRequired()
            .HasMaxLength(50);
    }
}