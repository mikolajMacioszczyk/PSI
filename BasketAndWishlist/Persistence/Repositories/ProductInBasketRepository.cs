using Application.Interfaces;
using Domain.Entities;

namespace Persistence.Repositories;

public class ProductInBasketRepository : IProductInBasketRepository
{
    private readonly BasketAndWishlistContext _context;

    public ProductInBasketRepository(BasketAndWishlistContext context)
    {
        _context = context;
    }

    public async Task<ProductInBasket> CreateAsync(ProductInBasket productInBasket)
    {
        await _context.AddAsync(productInBasket);
        return productInBasket;
    }

    public ProductInBasket Update(ProductInBasket productInBasket)
    {
        _context.Update(productInBasket);
        return productInBasket;
    }

    public void Remove(ProductInBasket productInBasket)
    {
        _context.Remove(productInBasket);
    }
}
