using Domain.Entities;

namespace Application.Interfaces;

public interface IProductInBasketRepository
{
    Task<ProductInBasket> CreateAsync(ProductInBasket productInBasket);
    ProductInBasket Update(ProductInBasket productInBasket);
    void Remove(ProductInBasket productInBasket);
}
