namespace Application.Models;

public class ProductInBasket
{
    public Guid Id { get; set; }
    public Guid BasketId { get; set; }
    public Guid ProductInCatalogId { get; set; }
    public int PieceCount { get; set; }
}
