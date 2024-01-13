using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC.Services.Interfaces;
using MVC.ViewModels.CatalogViewModels;
using MVC.ViewModels.Pagination;

namespace MVC.Controllers;

public class CatalogController : Controller
{
    private readonly ICatalogService _catalogService;
    private readonly ILogger<CatalogController> _logger;

    public CatalogController(ICatalogService catalogService,
        ILogger<CatalogController> logger)
    {
        _catalogService = catalogService;
        _logger = logger;
    }
    // [Authorize]
    public async Task<IActionResult> Index(int? itemsPage, int? page, int? brandFilter, int? typeFilter)
    {
        page ??= 0;
        itemsPage ??= 5;
        brandFilter ??= null;
        typeFilter ??= null;

        var catalog = await _catalogService.GetCatalogItems(page.Value,
            itemsPage.Value,
            brandFilter.HasValue ? brandFilter.Value + 1 : 0,
            typeFilter.HasValue ? typeFilter.Value + 1 : 0);

        if (catalog == null) return View("Error");

        var info = new PaginationInfo
        {
            ActualPage = page.Value,
            ItemsPerPage = catalog.Data.Count,
            TotalItems = catalog.Count,
            TotalPages = (int)Math.Ceiling((decimal)catalog.Count / (itemsPage ?? 5))
        };

        var vm = new IndexViewModel
        {
            CatalogItems = catalog.Data,
            Brands = await _catalogService.GetBrands(),
            Types = await _catalogService.GetTypes(),
            PaginationInfo = info
        };

        vm.PaginationInfo.Next = vm.PaginationInfo.ActualPage == vm.PaginationInfo.TotalPages - 1 ? "is-disabled" : "";
        vm.PaginationInfo.Previous = vm.PaginationInfo.ActualPage == 0 ? "is-disabled" : "";

        return View(vm);
    }
}