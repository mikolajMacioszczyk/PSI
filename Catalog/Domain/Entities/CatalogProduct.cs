namespace Domain.Entities
{
    public class CatalogProduct
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public required string SKU { get; set; }
        public decimal Price { get; set; }
        public required string PhotoUrl { get; set; }
        public required string Description { get; set; }
        public DateTime InCatalogFromTimestamp { get; set; }
        public DateTime? InCatalogToTimestamp { get; set; }
    }
}
