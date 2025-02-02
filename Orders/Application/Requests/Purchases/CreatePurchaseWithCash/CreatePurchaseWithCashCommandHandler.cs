using Application.Interfaces;
using AutoMapper;
using Common.Application.Interfaces;
using Common.Application.Models;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.Requests.Purchases.CreatePurchaseWithCash;

public class CreatePurchaseWithCashCommandHandler : IRequestHandler<CreatePurchaseWithCashCommand, Result<PurchaseResult>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IMapper _mapper;

    public CreatePurchaseWithCashCommandHandler(IUnitOfWork unitOfWork, IDateTimeProvider dateTimeProvider, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
        _mapper = mapper;
    }

    public async Task<Result<PurchaseResult>> Handle(CreatePurchaseWithCashCommand request, CancellationToken cancellationToken)
    {
        var order = await _unitOfWork.OrderRepository.GetByIdWithShipment(request.OrderId);

        if (order is null)
        {
            return new NotFound(request.OrderId, $"order with provided id {request.OrderId} not exists");
        }

        if (order.OrderStatus != OrderStatus.Submitted)
        {
            return new Failure($"Cannot create checkout session for order with status {order.OrderStatus}");
        }

        var purchaseAmount = order.OrderPrice + order.Shipment.ShipmentPrice;
        var purchase = new Purchase
        {
            Id = Guid.NewGuid(),
            Order = order,
            PaymentMethod = PaymentMethod.CashOnDelivery,
            Amount = purchaseAmount,
            PurchaseTimestamp = _dateTimeProvider.GetCurrentTime(),
        };

        await _unitOfWork.PurchaseRepository.CreateAsync(purchase);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<PurchaseResult>(purchase);
    }
}
