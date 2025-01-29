using Application.Interfaces;
using Domain.Entities;

namespace Persistence.Repositories;

public class BasketRepository : IBasketRepository
{
    private readonly BasketAndWishlistContext _context;

    public BasketRepository(BasketAndWishlistContext context)
    {
        _context = context;
    }

    public async Task<Basket> CreateAsync(Basket basket)
    {
        await _context.AddAsync(basket);
        return basket;
    }
}
