using Application.Models;

namespace Application.Interfaces;

public interface IShipmentPriceService
{
    Task<decimal> GetShipmentPrice(Basket basket);
}
