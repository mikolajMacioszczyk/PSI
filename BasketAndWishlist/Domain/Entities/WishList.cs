namespace Domain.Entities
{
    public class WishList
    {
        public Guid Id { get; set; }
        public Guid ClientId { get; set; }
        public ICollection<Guid> ProductsInWishList { get; set; } = [];
    }
}
