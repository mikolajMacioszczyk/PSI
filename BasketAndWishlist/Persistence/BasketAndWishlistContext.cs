using Microsoft.EntityFrameworkCore;
using Persistence.EntityConfigurations;

namespace Persistence;

public class BasketAndWishlistContext : DbContext
{
    public BasketAndWishlistContext(DbContextOptions<BasketAndWishlistContext> options) : base(options)
    {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new ProductInBasketEntityConfiguration());
    }
}
