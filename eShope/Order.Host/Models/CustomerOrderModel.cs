namespace Order.Host.Models;

public class CustomerOrderModel
{
    public int Id { get; set; }
    public string CustomerId { get; set; }
    public DateTime Date { get; set; }
    public decimal TotalPrice { get; set; }
}
