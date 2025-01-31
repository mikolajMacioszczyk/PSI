using Application.Models;

namespace Application.Interfaces;

public interface IShipmentService
{
    bool ValidateShipmentProviderExists(Guid providerId);
    Task<decimal> GetShipmentPrice(Basket basket);
}
