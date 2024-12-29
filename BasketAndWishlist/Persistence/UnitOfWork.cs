using Application.Interfaces;

namespace Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly BasketAndWishlistContext _context;

    public UnitOfWork(BasketAndWishlistContext context)
    {
        _context = context;
    }

    public event EventHandler? BeforeSaveChanges;

    public async Task<bool> SaveChangesAsync()
    {
        BeforeSaveChanges?.Invoke(this, EventArgs.Empty);

        return await _context.SaveChangesAsync() > 0;
    }
}
