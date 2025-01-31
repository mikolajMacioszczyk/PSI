using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Persistence.Repositories;

public class CatalogProductRepository : ICatalogProductRepository
{
    private readonly CatalogContext _context;

    public CatalogProductRepository(CatalogContext context)
    {
        _context = context;
    }

    public async Task<(ICollection<CatalogProduct>, int totalCount)> GetPaged(int pageNumber, int pageSize, Expression<Func<CatalogProduct, bool>>? filter = null)
    {
        IQueryable<CatalogProduct> query = _context.CatalogProducts;

        if (filter is not null)
        {
            query = query.Where(filter);
        }

        var totalCount = await query.CountAsync();

        var pagedCollection = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

        return (pagedCollection, totalCount);
    }

    public async Task<CatalogProduct?> GetById(Guid id)
    {
        return await _context.CatalogProducts.FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<CatalogProduct>> GetByIds(IEnumerable<Guid> ids)
    {
        return await _context.CatalogProducts.Where(p => ids.Contains(p.Id)).ToListAsync();
    }
}
