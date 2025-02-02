namespace Domain.Entities
{
    public class ProductInBasket
    {
        public Guid Id { get; set; }
        public Guid BasketId { get; set; }
        public required Basket Basket { get; set; }
        public Guid ProductInCatalogId { get; set; }
        public required string SKU { get; set; }
        public int PieceCount { get; set; }
    }
}
