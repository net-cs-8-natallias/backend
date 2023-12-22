using Catalog.Host.Data;
using Catalog.Host.Data.Entities;

namespace Catalog.Host.Services.Interfaces;

public interface IBffService
{
    Task<PaginatedItems<CatalogItem>> GetItems(int pageSize, int pageIndex);
    Task<PaginatedItems<CatalogBrand>> GetBrands(int pageSize, int pageIndex);
    Task<PaginatedItems<CatalogType>> GetTypes(int pageSize, int pageIndex);
    Task<CatalogItem> GetItem(int id);
    Task<CatalogBrand> GetBrand(int id);
    Task<CatalogType> GetType(int id);
    Task<List<CatalogItem>> GetItemByType(string type);
    Task<List<CatalogItem>> GetItemByBrand(string brand);
}