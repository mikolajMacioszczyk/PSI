using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations
{
    public class CatalogProductEntityConfiguration : IEntityTypeConfiguration<CatalogProduct>
    {
        public void Configure(EntityTypeBuilder<CatalogProduct> builder)
        {
            builder.Property(c => c.SKU).HasMaxLength(64);
            builder.Property(c => c.PhotoUrl).HasMaxLength(1024);
            builder.Property(c => c.Description).HasMaxLength(32768);
        }
    }
}
