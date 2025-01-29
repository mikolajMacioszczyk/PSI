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
}
