using Application.Interfaces;
using Application.Models;

namespace Application.Services;

public class OrderPriceService : IOrderPriceService
{
    private readonly ICatalogService _catalogService;

    public OrderPriceService(ICatalogService catalogService)
    {
        _catalogService = catalogService;
    }

    public async Task<decimal> GetOrderPrice(Basket basket)
    {
        var productsInCatalogIds = basket.ProductsInBaskets.Select(p => p.ProductInCatalogId).ToList();
        var catalogProducts = await _catalogService.GetCatalogProductsByIds(productsInCatalogIds);

        var result = basket.ProductsInBaskets.Sum(p => p.PieceCount * catalogProducts.First(c => c.Id == p.ProductInCatalogId).Price);
        return result;
    }
}
