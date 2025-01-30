using Application.Interfaces;
using Application.Models;

namespace Application.Services;

public class OrderPriceService : IOrderPriceService
{
    public Task<decimal> GetOrderPrice(Basket basket)
    {
        var productsInCatalogIds = basket.ProductsInBaskets.Select(p => p.ProductInCatalogId).ToList();
        // TODO
        var result = (decimal) basket.ProductsInBaskets.Sum(p => p.PieceCount);
        return Task.FromResult(result);
    }
}
