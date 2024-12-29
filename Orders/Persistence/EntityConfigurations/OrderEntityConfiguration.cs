using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations
{
    public class OrderEntityConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasOne(o => o.Purchase)
                .WithOne(p => p.Order)
                .HasForeignKey<Order>(o => o.PurchaseId);

            builder.HasOne(o => o.Shipment)
                .WithOne(s => s.Order)
                .HasForeignKey<Order>(o => o.ShipmentId);
        }
    }
}
