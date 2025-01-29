using Application.Requests.ProductInBaskets;

namespace Application.Requests.Baskets;

public class BasketResult
{
    public Guid Id { get; set; }
    public Guid? UserId { get; set; }
    public ICollection<ProductInBasketResult> ProductsInBaskets { get; set; } = [];
}
