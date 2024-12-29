using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations
{
    public class ShipmentEntityConfiguration : IEntityTypeConfiguration<Shipment>
    {
        public void Configure(EntityTypeBuilder<Shipment> builder)
        {
            builder.Property(s => s.FirstName)
                .HasMaxLength(128);

            builder.Property(s => s.LastName)
                .HasMaxLength(128);

            builder.Property(s => s.Email)
                .HasMaxLength(512);

            builder.Property(s => s.Country)
                .HasMaxLength(128);

            builder.Property(s => s.City)
                .HasMaxLength(128);

            builder.Property(s => s.Street)
                .HasMaxLength(128);

            builder.Property(s => s.PostalCode)
                .HasMaxLength(6);

            builder.Property(s => s.PhoneNumber)
                .HasMaxLength(12);

            builder.Property(s => s.PhoneNumber)
                .HasMaxLength(15);

            builder.Property(s => s.AreaCode)
                .HasMaxLength(3);

            builder.Property(s => s.TrackingLink)
                .HasMaxLength(1024);
        }
    }
}
