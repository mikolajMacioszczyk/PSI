namespace Application.Models;

public class CatalogProduct
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string Name { get; set; } = null!;
    public string SKU { get; set; } = null!;
    public decimal Price { get; set; }
    public string PhotoUrl { get; set; } = null!;
    public string Description { get; set; } = null!;
}
