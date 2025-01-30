﻿using Application.Interfaces;
using Application.Models;
using AutoMapper;
using Common.Application.Interfaces;
using Common.Application.Models;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.Requests.Orders.CreateOrder;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Result<OrderResult>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBasketService _basketService;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IOrderPriceService _orderPriceService;
    private readonly IOrderNumberService _orderNumberService;
    private readonly IShipmentPriceService _shipmentPriceService;
    private readonly IMapper _mapper;

    public CreateOrderCommandHandler(
        IUnitOfWork unitOfWork,
        IBasketService basketService,
        IDateTimeProvider dateTimeProvider,
        IOrderPriceService orderPriceService,
        IOrderNumberService orderNumberService,
        IShipmentPriceService shipmentPriceService,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _basketService = basketService;
        _dateTimeProvider = dateTimeProvider;
        _orderPriceService = orderPriceService;
        _orderNumberService = orderNumberService;
        _shipmentPriceService = shipmentPriceService;
        _mapper = mapper;
    }

    public async Task<Result<OrderResult>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var basket = await _basketService.GetBasketById(request.BasketId);

        if (basket is null)
        {
            return new Failure($"Basket with provided id {request.BasketId} not exists");
        }

        // TODO: Validate shipment provider

        var shipment = _mapper.Map<Shipment>(request);
        shipment.Id = Guid.NewGuid();
        shipment.ShipmentPrice = await _shipmentPriceService.GetShipmentPrice(basket);

        var order = new Order
        {
            Id = Guid.NewGuid(),
            BasketId = basket.Id,
            ClientId = GetClientId(basket),
            ShipmentId = shipment.Id,
            Shipment = shipment,
            OrderNumber = await _orderNumberService.GetNext(),
            OrderStatus = OrderStatus.Submitted,
            SubmitionTimestamp = _dateTimeProvider.GetCurrentTime(),
            ConsentGranted = true,
            OrderPrice = await _orderPriceService.GetOrderPrice(basket)
        };

        await _unitOfWork.OrderRepository.CreateAsync(order);

        await _unitOfWork.SaveChangesAsync();

        // TODO: Cleanup basket
        return _mapper.Map<OrderResult>(order);
    }

    private static Guid GetClientId(Basket basket) => basket.UserId ?? Guid.NewGuid();
}
