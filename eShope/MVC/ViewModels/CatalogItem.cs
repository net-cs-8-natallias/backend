namespace MVC.ViewModels;

public class CatalogItem
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public decimal Price { get; set; }

    public string PictureFileName { get; set; } = null!;

    public CatalogType CatalogType { get; set; } = null!;

    public CatalogBrand CatalogBrand { get; set; } = null!;

    public int AvailableStock { get; set; }
}