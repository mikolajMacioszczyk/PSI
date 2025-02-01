using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class PurchaseRepository : IPurchaseRepository
{
    private readonly OrdersContext _context;

    public PurchaseRepository(OrdersContext context)
    {
        _context = context;
    }

    public Task<Purchase?> GetById(Guid purchaseId)
    {
        return _context.Purchases.FirstOrDefaultAsync(o => o.Id == purchaseId);
    }

    public async Task<Purchase> CreateAsync(Purchase order)
    {
        await _context.AddAsync(order);
        return order;
    }
}
