namespace Catalog.Host.Data.Entities;

public class CatalogType
{
    public int Id { get; set; }
    public string? Type { get; set; }
    
    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }
        CatalogType other = (CatalogType)obj;
        return Id == other.Id &&
               Type == other.Type;
    }
    
    public override string ToString()
    {
        return $"CatalogType(Id: {Id}, Type: {Type}";
    }
}