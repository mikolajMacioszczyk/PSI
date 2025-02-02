namespace Application.Models;

public class ProductInBasket
{
    public Guid Id { get; set; }
    public Guid BasketId { get; set; }
    public Guid ProductInCatalogId { get; set; }
    public string SKU { get; set; } = null!;
    public int PieceCount { get; set; }
}
