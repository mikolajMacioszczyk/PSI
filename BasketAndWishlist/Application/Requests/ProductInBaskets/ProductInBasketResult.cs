namespace Application.Requests.ProductInBaskets;

public class ProductInBasketResult
{
    public Guid Id { get; set; }
    public Guid BasketId { get; set; }
    public Guid ProductInCatalogId { get; set; }
    public required string SKU { get; set; }
    public int PieceCount { get; set; }
}
