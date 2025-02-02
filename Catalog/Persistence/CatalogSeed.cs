using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Bogus;

namespace Persistence
{
    public static class CatalogSeed
    {
        public static async Task SeedDefaultProducts(CatalogContext catalogContext)
        {
            if (await catalogContext.CatalogProducts.AnyAsync())
            {
                return;
            }

            var faker = new Faker();

            var defaultCatalogProducts = Enumerable.Range(1, 100)
                .Select(i => 
                    new CatalogProduct 
                    {
                        Id = Guid.NewGuid(),
                        SKU = $"00000000-0000-0000-0000-{i.ToString().PadLeft(12, '0')}",
                        Name = faker.Commerce.ProductName(),
                        Price = Convert.ToDecimal(faker.Commerce.Price()),
                        Description = faker.Commerce.ProductDescription(),
                        InCatalogFromTimestamp = DateTime.UtcNow,
                        InCatalogToTimestamp = null,
                        PhotoUrl = "https://psediting.websites.co.in/obaju-turquoise/img/product-placeholder.png"
                    });

            await catalogContext.CatalogProducts.AddRangeAsync(defaultCatalogProducts);
            await catalogContext.SaveChangesAsync();
        }
    }
}
