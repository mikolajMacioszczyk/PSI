using Application.Interfaces;
using Persistence.Repositories;

namespace Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly BasketAndWishlistContext _context;

    public IBasketRepository BasketRepository { get; }
    public IProductInBasketRepository ProductInBasketRepository { get; }

    public UnitOfWork(BasketAndWishlistContext context)
    {
        _context = context;
        BasketRepository = new BasketRepository(context);
        ProductInBasketRepository = new ProductInBasketRepository(context);
    }

    public event EventHandler? BeforeSaveChanges;

    public async Task<bool> SaveChangesAsync()
    {
        BeforeSaveChanges?.Invoke(this, EventArgs.Empty);

        return await _context.SaveChangesAsync() > 0;
    }
}
