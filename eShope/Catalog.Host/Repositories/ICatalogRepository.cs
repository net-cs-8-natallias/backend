using Catalog.Host.Data;
using Catalog.Host.Data.Entities;
using Catalog.Host.Models;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Host.Repositories;

public interface ICatalogRepository<T>
{
    Task<PaginatedItems<T>> GetCatalog(int pageSize, int pageIndex);
    Task<T> FindById(int id);
    Task<int?> AddToCatalog(T catalogItem);
    Task<T> UpdateInCatalog(T catalogItem);
    Task<T> RemoveFromCatalog(int id);

}