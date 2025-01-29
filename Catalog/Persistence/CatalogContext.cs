using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.EntityConfigurations;

namespace Persistence;

public class CatalogContext : DbContext
{
    public DbSet<CatalogProduct> CatalogProducts { get; set; }
    public CatalogContext(DbContextOptions<CatalogContext> options) : base(options)
    {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new CatalogProductEntityConfiguration());
    }
}
