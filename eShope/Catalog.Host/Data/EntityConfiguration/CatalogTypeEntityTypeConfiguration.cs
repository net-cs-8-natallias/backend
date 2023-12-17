using Catalog.Host.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Host.Data.EntityConfiguration;

public class CatalogTypeEntityTypeConfiguration: IEntityTypeConfiguration<CatalogType>
{
    public void Configure(EntityTypeBuilder<CatalogType> builder)
    {
        builder.ToTable("CatalogType");
        builder.HasKey(catalogType => catalogType.Id);
        builder.Property(catalogType => catalogType.Id)
            .UseHiLo("catalog_type_hilo")
            .IsRequired();

        builder.Property((catalogType => catalogType.Type))
            .IsRequired()
            .HasMaxLength(50);
        // builder.Property(catalogBrand => catalogBrand.Type)
        //     .IsRequired()
        //     .HasMaxLength(50);
    }
}