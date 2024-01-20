using Catalog.Host.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Host.Data.EntityConfiguration;

public class CatalogItemEntityTypeConfiguration : IEntityTypeConfiguration<CatalogItem>
{
    public void Configure(EntityTypeBuilder<CatalogItem> builder)
    {
        builder.ToTable("catalogitem");
   
        builder.Property(catalogItem => catalogItem.Id).HasColumnName("id")
            .IsRequired();

        builder.Property(catalogItem => catalogItem.Name).HasColumnName("name")
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(catalogItem => catalogItem.Price).HasColumnName("price")
            .IsRequired();

        builder.Property(catalogItem => catalogItem.PictureFileName).HasColumnName("picturefilename")
            .IsRequired(false);

        builder.Property(item => item.AvailableStock).HasColumnName("availablestock");

        builder.Property(item => item.Description).HasColumnName("description")
            .IsRequired(false);

        builder.HasOne(catalogItem => catalogItem.CatalogBrand)
            .WithMany()
            .HasForeignKey(catalogItem => catalogItem.CatalogBrandId);
        builder.Property(item => item.CatalogBrandId).HasColumnName("catalogbrandid");

        builder.HasOne(catalogItem => catalogItem.CatalogType)
            .WithMany()
            .HasForeignKey(catalogItem => catalogItem.CatalogTypeId);
        builder.Property(item => item.CatalogTypeId).HasColumnName("catalogtypeid");
    }
}