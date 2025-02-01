using Application.Interfaces;
using Common.Application.Models;
using MediatR;

namespace Application.Requests.Orders.GetExpectedOrderCost;

public class GetExpectedOrderCostQueryHandler : IRequestHandler<GetExpectedOrderCostQuery, Result<GetExpectedOrderCostQueryResult>>
{
    private readonly IBasketService _basketService;
    private readonly IOrderPriceService _orderPriceService;
    private readonly IShipmentService _shipmentService;

    public GetExpectedOrderCostQueryHandler(
        IBasketService basketService,
        IOrderPriceService orderPriceService,
        IShipmentService shipmentService)
    {
        _basketService = basketService;
        _orderPriceService = orderPriceService;
        _shipmentService = shipmentService;
    }

    public async Task<Result<GetExpectedOrderCostQueryResult>> Handle(GetExpectedOrderCostQuery request, CancellationToken cancellationToken)
    {
        var basket = await _basketService.GetBasketById(request.BasketId);

        if (basket is null)
        {
            return new Failure($"Basket with provided id {request.BasketId} not exists");
        }

        if (!basket.IsActive)
        {
            return new Failure($"Basket with provided id {request.BasketId} is not active");
        }

        var shipmentPrice = await _shipmentService.GetShipmentPrice(basket);
        var orderPrice = await _orderPriceService.GetOrderPrice(basket);

        return new GetExpectedOrderCostQueryResult
        {
            ShipmentPrice = shipmentPrice,
            OrderPrice = orderPrice,
            TotalPrice = orderPrice + shipmentPrice,
        };
    }
}
