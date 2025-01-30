using Application.Interfaces;
using Application.Models;

namespace Application.Services;

public class ShipmentPriceService : IShipmentPriceService
{
    public Task<decimal> GetShipmentPrice(Basket basket)
    {
        // TODO
        return Task.FromResult(4m);
    }
}
