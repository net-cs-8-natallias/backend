namespace Infrastructure.RateLimit.models;

public class KeyModel
{
    public string Ip { get; set; } = null!;
    public string Endpoint { get; set; } = null!;
}