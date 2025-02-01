using Application.Interfaces;
using Persistence.Repositories;

namespace Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly OrdersContext _context;

    public IOrderRepository OrderRepository { get; }

    public UnitOfWork(OrdersContext context)
    {
        _context = context;
        OrderRepository = new OrderRepository(context);
    }

    public event EventHandler? BeforeSaveChanges;

    public async Task<bool> SaveChangesAsync()
    {
        BeforeSaveChanges?.Invoke(this, EventArgs.Empty);

        return await _context.SaveChangesAsync() > 0;
    }
}
