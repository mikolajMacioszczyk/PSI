using Application.Interfaces;
using Application.Models;

namespace Application.Services;

public class OrderPriceService : IOrderPriceService
{
    public async Task<decimal> GetOrderPrice(Basket basket)
    {
        var productsInCatalogIds = basket.ProductsInBaskets.Select(p => p.ProductInCatalogId).ToList();
        // TODO
        return basket.ProductsInBaskets.Sum(p => p.PieceCount);
    }
}
