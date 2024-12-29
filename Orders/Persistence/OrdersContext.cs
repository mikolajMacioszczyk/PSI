using Microsoft.EntityFrameworkCore;
using Persistence.EntityConfigurations;

namespace Persistence;

public class OrdersContext : DbContext
{
    public OrdersContext(DbContextOptions<OrdersContext> options) : base(options)
    {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new OrderEntityConfiguration());
        modelBuilder.ApplyConfiguration(new OrderStatusChangedNotificationEntityConfiguration());
        modelBuilder.ApplyConfiguration(new ShipmentEntityConfiguration());
    }
}
