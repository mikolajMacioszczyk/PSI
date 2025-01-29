using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class CatalogProductRepository : ICatalogProductRepository
{
    private readonly CatalogContext _context;

    public CatalogProductRepository(CatalogContext context)
    {
        _context = context;
    }

    public async Task<ICollection<CatalogProduct>> GetAll()
    {
        return await _context.CatalogProducts.ToListAsync();
    }

    public async Task<(ICollection<CatalogProduct>, int totalCount)> GetPaged(int pageNumber, int pageSize)
    {
        var totalCount = await _context.CatalogProducts.CountAsync();

        var pagedCollection = await _context.CatalogProducts.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

        return (pagedCollection, totalCount);
    }
}
