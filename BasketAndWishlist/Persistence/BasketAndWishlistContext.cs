using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.EntityConfigurations;

namespace Persistence;

public class BasketAndWishlistContext : DbContext
{
    public DbSet<Basket> Baskets { get; set; }
    public DbSet<ProductInBasket> ProductsInBaskets { get; set; }
    public DbSet<WishList> WishLists { get; set; }

    public BasketAndWishlistContext(DbContextOptions<BasketAndWishlistContext> options) : base(options)
    {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new ProductInBasketEntityConfiguration());
    }
}
