using Order.Host.Data.Entities;

namespace Order.Host.Data;

public class CustomerDbInitializer
{
    public static async Task Initialize(ApplicationDbContext context)
    {
        await context.Database.EnsureCreatedAsync();
        
        if (!context.Customers.Any())
        {
            await context.Customers.AddRangeAsync(GetPreconfiguredCustumers());
            await context.SaveChangesAsync();
        }
        
        if (!context.Orders.Any())
        {
            await context.Orders.AddRangeAsync(GetPreconfiguredOrders());
            await context.SaveChangesAsync();
        }
        
        if (!context.OrderItems.Any())
        {
            await context.OrderItems.AddRangeAsync(GetPreconfiguredOrderItems());
            await context.SaveChangesAsync();
        }
    }

    private static IEnumerable<Customer> GetPreconfiguredCustumers()
    {
        return new List<Customer>
        {
            new() {Id = "1", Name = "Customer1", Email = "customer1@gmail.com" },
            new() {Id = "2", Name = "Customer2", Email = "customer2@gmail.com" },
            new() {Id = "3", Name = "Customer3", Email = "customer3@gmail.com" }
        };
    } 
    
    private static IEnumerable<CustomerOrder> GetPreconfiguredOrders()
    {
        return new List<CustomerOrder>
        {
            new()
            {
                CustomerId = "1",
                Date = new DateTime(),
                TotalPrice = 150
            }
        };
    }

    private static IEnumerable<OrderItem> GetPreconfiguredOrderItems()
    {
        return new List<OrderItem>
        {
            new() {CustomerOrderId = 1, Amount = 5, Subtotal = 55 }
        };
    }
}