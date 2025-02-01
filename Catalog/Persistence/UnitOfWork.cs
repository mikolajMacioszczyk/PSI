using Application.Interfaces;
using Persistence.Repositories;

namespace Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly CatalogContext _context;
    public ICatalogProductRepository CatalogProductRepository { get; }

    public UnitOfWork(CatalogContext context)
    {
        _context = context;
        CatalogProductRepository = new CatalogProductRepository(context);
    }

    public event EventHandler? BeforeSaveChanges;

    public async Task<bool> SaveChangesAsync()
    {
        BeforeSaveChanges?.Invoke(this, EventArgs.Empty);

        return await _context.SaveChangesAsync() > 0;
    }
}
