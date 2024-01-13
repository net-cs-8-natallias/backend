namespace Catalog.Host.Data.Entities;

public class CatalogItem
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public string? PictureFileName { get; set; }
    public int AvailableStock { get; set; }
    public int CatalogTypeId { get; set; }
    public int CatalogBrandId { get; set; }
    public CatalogType? CatalogType { get; set; }
    public CatalogBrand? CatalogBrand { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType()) return false;
        var other = (CatalogItem)obj;
        return Id == other.Id &&
               Name == other.Name &&
               Description == other.Description &&
               Price == other.Price &&
               PictureFileName == other.PictureFileName &&
               CatalogBrandId == other.CatalogBrandId &&
               CatalogTypeId == other.CatalogTypeId;
    }

    public override string ToString()
    {
        return $"CatalogItem(Id: {Id}, Name: {Name}, Description: {Description}, " +
               $"Price: {Price}, PictureFileName: {PictureFileName}, " +
               $"AvailableStock: {AvailableStock}, CatalogTypeId: {CatalogTypeId}, " +
               $"CatalogBrandId: {CatalogBrandId}";
    }
}