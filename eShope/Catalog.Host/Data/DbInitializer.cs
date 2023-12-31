using Catalog.Host.Data.Entities;

namespace Catalog.Host.Data;

public static class DbInitializer
{
    public static async Task Initialize(ApplicationDbContext context)
    {
        await context.Database.EnsureCreatedAsync();

        if (!context.CatalogBrands.Any())
        {
            await context.CatalogBrands.AddRangeAsync(GetPreconfiguredCatalogBrands());

            await context.SaveChangesAsync();
        }

        if (!context.CatalogTypes.Any())
        {
            await context.CatalogTypes.AddRangeAsync(GetPreconfiguredCatalogTypes());

            await context.SaveChangesAsync();
        }

        if (!context.CatalogItems.Any())
        {
            await context.CatalogItems.AddRangeAsync(GetPreconfiguredItems());

            await context.SaveChangesAsync();
        }
    }

    private static IEnumerable<CatalogBrand> GetPreconfiguredCatalogBrands()
    {
        return new List<CatalogBrand>
        {
            new() { Brand = "Azure" },
            new() { Brand = ".NET" },
            new() { Brand = "Visual Studio" },
            new() { Brand = "SQL Server" },
            new() { Brand = "Other" }
        };
    }

    private static IEnumerable<CatalogType> GetPreconfiguredCatalogTypes()
    {
        return new List<CatalogType>
        {
            new() { Type = "Mug" },
            new() { Type = "T-Shirt" },
            new() { Type = "Sheet" },
            new() { Type = "USB Memory Stick" }
        };
    }

    private static IEnumerable<CatalogItem> GetPreconfiguredItems()
    {
        return new List<CatalogItem>
        {
            new()
            {
                CatalogTypeId = 2, CatalogBrandId = 2, AvailableStock = 100, Description = ".NET Bot Black Hoodie",
                Name = ".NET Bot Black Hoodie", Price = 19.5M, PictureFileName = "1.png"
            },
            new()
            {
                CatalogTypeId = 1, CatalogBrandId = 2, AvailableStock = 100, Description = ".NET Black & White Mug",
                Name = ".NET Black & White Mug", Price = 8.50M, PictureFileName = "2.png"
            },
            new()
            {
                CatalogTypeId = 2, CatalogBrandId = 5, AvailableStock = 100, Description = "Prism White T-Shirt",
                Name = "Prism White T-Shirt", Price = 12, PictureFileName = "3.png"
            },
            new()
            {
                CatalogTypeId = 2, CatalogBrandId = 2, AvailableStock = 100, Description = ".NET Foundation T-shirt",
                Name = ".NET Foundation T-shirt", Price = 12, PictureFileName = "4.png"
            },
            new()
            {
                CatalogTypeId = 3, CatalogBrandId = 5, AvailableStock = 100, Description = "Roslyn Red Sheet",
                Name = "Roslyn Red Sheet", Price = 8.5M, PictureFileName = "5.png"
            },
            new()
            {
                CatalogTypeId = 2, CatalogBrandId = 2, AvailableStock = 100, Description = ".NET Blue Hoodie",
                Name = ".NET Blue Hoodie", Price = 12, PictureFileName = "6.png"
            },
            new()
            {
                CatalogTypeId = 2, CatalogBrandId = 5, AvailableStock = 100, Description = "Roslyn Red T-Shirt",
                Name = "Roslyn Red T-Shirt", Price = 12, PictureFileName = "7.png"
            },
            new()
            {
                CatalogTypeId = 2, CatalogBrandId = 5, AvailableStock = 100, Description = "Kudu Purple Hoodie",
                Name = "Kudu Purple Hoodie", Price = 8.5M, PictureFileName = "8.png"
            },
            new()
            {
                CatalogTypeId = 1, CatalogBrandId = 5, AvailableStock = 100, Description = "Cup<T> White Mug",
                Name = "Cup<T> White Mug", Price = 12, PictureFileName = "9.png"
            },
            new()
            {
                CatalogTypeId = 3, CatalogBrandId = 2, AvailableStock = 100, Description = ".NET Foundation Sheet",
                Name = ".NET Foundation Sheet", Price = 12, PictureFileName = "10.png"
            },
            new()
            {
                CatalogTypeId = 3, CatalogBrandId = 2, AvailableStock = 100, Description = "Cup<T> Sheet",
                Name = "Cup<T> Sheet", Price = 8.5M, PictureFileName = "11.png"
            },
            new()
            {
                CatalogTypeId = 2, CatalogBrandId = 5, AvailableStock = 100, Description = "Prism White TShirt",
                Name = "Prism White TShirt", Price = 12, PictureFileName = "12.png"
            }
        };
    }
}