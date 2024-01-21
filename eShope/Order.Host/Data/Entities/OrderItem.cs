namespace Order.Host.Data.Entities;

public class OrderItem
{
    public int Id { get; set; }
    public int? CustomerOrderId { get; set; }
    public CustomerOrder? CustomerOrder { get; set; }
    public int Amount { get; set; }
    public decimal Subtotal { get; set; }
    public int CatalogItemId { get; set; }
}