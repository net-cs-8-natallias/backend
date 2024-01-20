using Microsoft.EntityFrameworkCore;
using Order.Host.Data.Entities;
using Order.Host.Data.EntitiesConfigurations;


namespace Order.Host.Data;

public class ApplicationDbContext: DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<Customer> Customers { get; set; }
    public DbSet<CustomerOrder> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    //public DbSet<CatalogItem> CatalogItem { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new CustomerEntityTypeConfiguration());
        builder.ApplyConfiguration(new OrderEntityTypeConfiguration());
        builder.ApplyConfiguration(new OrderItemEntityTypeConfiguration());
        //builder.ApplyConfiguration(new CatalogItemEntityTypeConfiguration());
    }
}