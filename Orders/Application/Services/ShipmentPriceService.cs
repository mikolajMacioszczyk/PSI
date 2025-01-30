using Application.Interfaces;
using Application.Models;

namespace Application.Services;

public class ShipmentPriceService : IShipmentPriceService
{
    public async Task<decimal> GetShipmentPrice(Basket basket)
    {
        // TODO
        return 4;
    }
}
