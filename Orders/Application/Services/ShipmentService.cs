using Application.Interfaces;
using Application.Models;

namespace Application.Services;

// TODO
public class ShipmentService : IShipmentService
{
    public bool ValidateShipmentProviderExists(Guid providerId)
    {
        return true;
    }

    public Task<decimal> GetShipmentPrice(Basket basket)
    {
        return Task.FromResult(4m);
    }
}
