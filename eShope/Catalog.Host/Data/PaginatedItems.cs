namespace Catalog.Host.Data;

public class PaginatedItems<T>
{
    public long Count { get; set; }
    public IEnumerable<T> Data { get; init; } = null!;
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
}