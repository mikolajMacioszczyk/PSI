using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations
{
    public class OrderStatusChangedNotificationEntityConfiguration : IEntityTypeConfiguration<OrderStatusChangedNotification>
    {
        public void Configure(EntityTypeBuilder<OrderStatusChangedNotification> builder)
        {
            builder.HasOne(n => n.Order)
                .WithMany(o => o.StatusChangedNotifications)
                .HasForeignKey(n => n.OrderId);

            builder.Property(o => o.Text)
                .HasMaxLength(32768);
        }
    }
}
