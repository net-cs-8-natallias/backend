using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order.Host.Data.Entities;

namespace Order.Host.Data.EntitiesConfigurations;

public class CustomerEntityTypeConfiguration: IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("customer");
        builder.HasKey(customer => customer.Id);
        builder.Property(customer => customer.Id).HasColumnName("id")
            .IsRequired();

        builder.Property(customer => customer.Name).HasColumnName("name")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(customer => customer.Email).HasColumnName("email")
            .HasMaxLength(50)
            .IsRequired();
    }
}