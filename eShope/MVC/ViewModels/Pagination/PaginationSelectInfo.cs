using Microsoft.AspNetCore.Mvc.Rendering;

namespace MVC.ViewModels.Pagination;

public class PaginationSelectInfo
{
    public IEnumerable<SelectListItem>? Page { get; set; }
    public IEnumerable<SelectListItem>? ItemsPage { get; set; }
}