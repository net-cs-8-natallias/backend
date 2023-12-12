using Microsoft.AspNetCore.Mvc;

namespace Catalog.Host.Controllers;

[ApiController]
[Route("[controller]")] // -> localhost/WeatherForecast
//[Route("[controller]/[action]")] // -> localhost/WeatherForecast/get
public class WeatherForecastController : ControllerBase
{
    private readonly IStringReader _stringReader;
    private readonly string[] _summaries = 
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    public WeatherForecastController(IStringReader stringReader)
    {
        this._stringReader = stringReader;
    }
    
    [HttpGet(Name = "GetWeatherForecast")] // Name -> metodata
    public IEnumerable<WeatherForecast> Get()
    {
        var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    _summaries[Random.Shared.Next(_summaries.Length)]
                ))
            .ToArray();
        return forecast;
    }
    
    [HttpGet("test")] // -> localhost/WeatherForecast/test
    public string GetUsers()
    {
        return _stringReader.ReadString();
    }

    public class Product
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public double Price { get; set; }
    }
    
    private readonly Product[] _products = new Product[]
    {
        new Product { Id = 1, Name = "Product A", Price = 20},
        new Product { Id = 2, Name = "Product B", Price = 30},
        new Product { Id = 3, Name = "Product C", Price = 40},
    };
    
    [HttpGet("products")]
    public IEnumerable<Product> GetProducts()
    {
        return _products;
    }

}