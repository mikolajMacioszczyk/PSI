using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

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

    public Task<Basket?> GetByIdWithProducts(Guid id)
    {
        return _context.Baskets
            .Include(b => b.ProductsInBaskets)
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public Task<Basket?> GetByUserIdWithProducts(Guid userId)
    {
        return _context.Baskets
            .Include(b => b.ProductsInBaskets)
            .FirstOrDefaultAsync(b => b.UserId == userId);
    }
}
