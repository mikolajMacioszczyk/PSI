using Domain.Entities;

namespace Application.Requests.Products;

public static class CatalogProductExtensions
{
    public static bool IsActive(this CatalogProduct catalogProduct)
    {
        return catalogProduct.InCatalogToTimestamp == null || catalogProduct.InCatalogToTimestamp < DateTime.UtcNow;
    }
}
