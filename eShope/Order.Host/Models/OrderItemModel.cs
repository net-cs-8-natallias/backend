namespace Order.Host.Models;

public class OrderItemModel
{
    public int Id { get; set; }
    public int? CustomerOrderId { get; set; }
    public int Amount { get; set; }
    public decimal Subtotal { get; set; }
    public int CatalogItemId { get; set; }
}