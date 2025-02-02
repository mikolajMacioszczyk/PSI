namespace Application.Models;

public class CatalogProduct
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string SKU { get; set; }
    public decimal Price { get; set; }
    public required string PhotoUrl { get; set; }
    public required string Description { get; set; }
}
