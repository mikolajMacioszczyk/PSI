using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.EntityConfigurations;

namespace Persistence;

public class OrdersContext : DbContext
{
    public DbSet<Order> Orders { get; set; }
    public DbSet<Shipment> Shipments { get; set; }
    public DbSet<Purchase> Purchases { get; set; }
    public DbSet<OrderStatusChangedNotification> OrderStatusChangedNotifications { get; set; }
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
