using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order.Host.Data.Entities;

namespace Order.Host.Data.EntitiesConfigurations;

public class OrderEntityTypeConfiguration: IEntityTypeConfiguration<CustomerOrder>
{
    public void Configure(EntityTypeBuilder<CustomerOrder> builder)
    {
        builder.ToTable("customerorder");
        builder.HasKey(order => order.Id);
        builder.Property(order => order.Id).HasColumnName("id")
            .UseIdentityColumn() 
            .IsRequired();

        builder.Property(order => order.Date).HasColumnName("date")
            .IsRequired();

        builder.Property(order => order.TotalPrice).HasColumnName("totalprice")
            .HasColumnType("decimal(18, 2)")
            .IsRequired();

        builder.Property(order => order.CustomerId).HasColumnName("customerid");
        builder.HasOne(order => order.Customer)
            .WithMany()
            .HasForeignKey(order => order.CustomerId);
    }
}
