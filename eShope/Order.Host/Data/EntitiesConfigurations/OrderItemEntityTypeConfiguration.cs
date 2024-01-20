using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order.Host.Data.Entities;

namespace Order.Host.Data.EntitiesConfigurations;

public class OrderItemEntityTypeConfiguration: IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("orderitem");
        builder.HasKey(item => item.Id);
        builder.Property(item => item.Id).HasColumnName("id")
            .UseIdentityColumn() 
            .IsRequired();

        builder.Property(item => item.Amount).HasColumnName("amount")
            .IsRequired();

        builder.Property(item => item.Subtotal).HasColumnName("subtotal")
            .HasColumnType("decimal(18, 2)")
            .IsRequired();

        builder.Property(item => item.CatalogItemId).HasColumnName("catalogitemid")
            .IsRequired();

        builder.Property(item => item.CustomerOrderId).HasColumnName("customerorderid");
        
        builder.HasOne(item => item.CustomerOrder)
            .WithMany()
            .HasForeignKey(item => item.CustomerOrderId);
        
    }
}
