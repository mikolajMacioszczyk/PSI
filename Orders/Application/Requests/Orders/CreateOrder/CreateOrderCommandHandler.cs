using Application.Interfaces;
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
    private readonly IMapper _mapper;

    public CreateOrderCommandHandler(
        IUnitOfWork unitOfWork, 
        IBasketService basketService, 
        IDateTimeProvider dateTimeProvider, 
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _basketService = basketService;
        _dateTimeProvider = dateTimeProvider;
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

        var shipment = new Shipment
        {
            Id = Guid.NewGuid(),
            ProviderId = request.ShipmentProviderId,
            ShipmentPrice = GetShipmentPrice(basket),
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Country = request.Country,
            City = request.City,
            Street = request.Street,
            PostalCode = request.PostalCode,
            HomeNumber = request.HomeNumber,
            PhoneNumber = request.PhoneNumber,
            AreaCode = request.AreaCode,
            TrackingLink = string.Empty
        };

        var order = new Order
        {
            Id = Guid.NewGuid(),
            BasketId = basket.Id,
            ClientId = GetClientId(basket),
            ShipmentId = shipment.Id,
            Shipment = shipment,
            OrderNumber = GetOrderNumber(),
            OrderStatus = OrderStatus.Submitted,
            SubmitionTimestamp = _dateTimeProvider.GetCurrentTime(),
            ConsentGranted = true,
            OrderPrice = GetOrderPrice(basket)
        };

        await _unitOfWork.OrderRepository.CreateAsync(order);

        await _unitOfWork.SaveChangesAsync();

        // TODO: Cleanup basket
        return _mapper.Map<OrderResult>(order);
    }

    private static Guid GetClientId(Basket basket) => basket.UserId ?? Guid.NewGuid();

    // TODO: Order number service
    private static string GetOrderNumber() => "ZM_TODO";
    // TODO: service
    private static decimal GetOrderPrice(Basket basket) => 10;
    // TODO: service
    private static decimal GetShipmentPrice(Basket basket) => 2;
}
