using Application.Interfaces;
using Application.Models;

namespace Application.Services;

public class ShipmentService : IShipmentService
{
    private readonly IInventoryService _inventoryService;
    private readonly IBasketService _basketService;

    public ShipmentService(IInventoryService inventoryService, IBasketService basketService)
    {
        _inventoryService = inventoryService;
        _basketService = basketService;
    }

    public bool ValidateShipmentProviderExists(Guid providerId)
    {
        return true;
    }

    public Task<decimal> GetShipmentPrice(Basket basket)
    {
        // TODO
        return Task.FromResult(4m);
    }
}
