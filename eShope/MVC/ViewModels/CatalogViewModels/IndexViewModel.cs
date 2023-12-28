using Microsoft.AspNetCore.Mvc.Rendering;
using MVC.ViewModels.Pagination;

namespace MVC.ViewModels.CatalogViewModels;

public class IndexViewModel
{
    public IEnumerable<CatalogItem>? CatalogItems { get; set; }
    public IEnumerable<SelectListItem>? Brands { get; set; }
    public IEnumerable<SelectListItem>? Types { get; set; }
    public int? BrandFilter { get; set; }
    public int? TypeFilter { get; set; }
    public PaginationInfo? PaginationInfo { get; set; }
}