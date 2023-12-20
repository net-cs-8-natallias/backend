namespace Catalog.Host.Data.Entities;

public class CatalogBrand
{
    public int Id { get; set; }
    public string? Brand { get; set; }
    
    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }
        CatalogBrand other = (CatalogBrand)obj;
        return Id == other.Id &&
               Brand == other.Brand;
    }
    
    public override string ToString()
    {
        return $"CatalogBrand(Id: {Id}, Brand: {Brand}";
    }
}