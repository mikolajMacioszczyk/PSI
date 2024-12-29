using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations
{
    public class ProductInBasketEntityConfiguration : IEntityTypeConfiguration<ProductInBasket>
    {
        public void Configure(EntityTypeBuilder<ProductInBasket> builder)
        {
            builder.HasOne(p => p.Basket)
                .WithMany(b => b.ProductsInBaskets)
                .HasForeignKey(p => p.BasketId);
        }
    }
}
