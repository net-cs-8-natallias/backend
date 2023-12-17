using Catalog.Host.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Host.Data.EntityConfiguration;

public class CatalogItemEntityTypeConfiguration: IEntityTypeConfiguration<CatalogItem>
{
    public void Configure(EntityTypeBuilder<CatalogItem> builder)
    {
        builder.ToTable("CatalogItem");
        //builder.HasKey(catalogItem => catalogItem.Id);
        builder.Property(catalogItem => catalogItem.Id)
            .UseHiLo("catalog_item_hilo")
            .IsRequired();

        builder.Property((catalogItem => catalogItem.Name))
            .IsRequired(true)
            .HasMaxLength(50);

        builder.Property((catalogItem => catalogItem.Price))
            .IsRequired();

        builder.Property((catalogItem => catalogItem.PictureFileName))
            .IsRequired(false);

        builder.HasOne(catalogItem => catalogItem.CatalogBrand)
            .WithMany()
            .HasForeignKey(catalogItem => catalogItem.CatalogBrandId);
        
        builder.HasOne(catalogItem => catalogItem.CatalogType)
            .WithMany()
            .HasForeignKey(catalogItem => catalogItem.CatalogTypeId);
        

    }
}