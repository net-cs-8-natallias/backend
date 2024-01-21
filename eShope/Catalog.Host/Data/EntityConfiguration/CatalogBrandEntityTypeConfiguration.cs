using Catalog.Host.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Host.Data.EntityConfiguration;

public class CatalogBrandEntityTypeConfiguration : IEntityTypeConfiguration<CatalogBrand>
{
    public void Configure(EntityTypeBuilder<CatalogBrand> builder)
    {
        builder.ToTable("catalogbrand");
        builder.HasKey(catalogBrand => catalogBrand.Id);
        builder.Property(catalogBrand => catalogBrand.Id).HasColumnName("id")
            .UseIdentityColumn() 
            .IsRequired();

        builder.Property(catalogBrand => catalogBrand.Brand).HasColumnName("brand")
            .IsRequired()
            .HasMaxLength(50);
    }
}