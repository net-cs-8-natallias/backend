using System.ComponentModel.DataAnnotations.Schema;

namespace Order.Host.Data.Entities;

public class Customer
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

}