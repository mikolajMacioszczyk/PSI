namespace Domain.Entities
{
    public class Basket
    {
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public ICollection<ProductInBasket> ProductsInBaskets { get; set; } = [];
    }
}
