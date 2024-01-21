namespace Order.Host.Data.Entities;

public class CustomerOrder
{
    public int Id { get; set; }
    public string CustomerId { get; set; }
    public Customer? Customer { get; set; }
    public DateTime Date { get; set; }
    public decimal TotalPrice { get; set; }

}