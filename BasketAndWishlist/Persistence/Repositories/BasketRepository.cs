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

    public Task<Basket?> GetById(Guid id)
    {
        return _context.Baskets
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public Task<Basket?> GetByIdWithProducts(Guid id)
    {
        return _context.Baskets
            .Include(b => b.ProductsInBaskets)
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public Task<Basket?> GetActiveByUserIdWithProducts(Guid userId)
    {
        return _context.Baskets
            .Include(b => b.ProductsInBaskets)
            .FirstOrDefaultAsync(b => b.UserId == userId && b.IsActive);
    }

    public void Update(Basket basket)
    {
        _context.Baskets.Update(basket);
    }
}
