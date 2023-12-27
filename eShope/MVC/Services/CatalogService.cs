using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using MVC.Services.Interfaces;
using MVC.ViewModels;
using System.Linq;

namespace MVC.Services;

public class CatalogService : ICatalogService
{
    private readonly IOptions<AppSettings> _settings;
    private readonly IHttpClientService _httpClient;
    private readonly ILogger<CatalogService> _logger;

    public CatalogService(IHttpClientService httpClient, ILogger<CatalogService> logger, IOptions<AppSettings> settings)
    {
        _httpClient = httpClient;
        _settings = settings;
        _logger = logger;
    }

    public async Task<Catalog> GetCatalogItems(int page, int items, int brand, int type)
    {
        string query = $"pageIndex={page}&pageSize={items}&brand={brand}&type={type}";
        var result = await _httpClient
            .SendAsync<Catalog, object>($"{_settings.Value.CatalogUrl}/items?{query}",
           HttpMethod.Get, null);
        return result;
    }

    public async Task<IEnumerable<SelectListItem>> GetBrands()
    {
        var result = await _httpClient
            .SendAsync<IEnumerable<CatalogBrand>, object>($"{_settings.Value.CatalogUrl}/brands",
                HttpMethod.Get, null);
        
        return result.Select((brand, index) => new SelectListItem { Value = index.ToString(), Text = brand.Brand });
    }

    public async Task<IEnumerable<SelectListItem>> GetTypes()
    {
        var result = await _httpClient
            .SendAsync<IEnumerable<CatalogType>, object>($"{_settings.Value.CatalogUrl}/types",
                HttpMethod.Get, null);
        
        return result.Select((type, index) => new SelectListItem { Value = index.ToString(), Text = type.Type });
    }
}