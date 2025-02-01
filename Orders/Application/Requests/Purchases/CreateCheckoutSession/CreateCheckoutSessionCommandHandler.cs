using Application.Interfaces;
using Common.Application.Interfaces;
using Common.Application.Models;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.Requests.Purchases.CreateCheckoutSession;

public class CreateCheckoutSessionCommandHandler : IRequestHandler<CreateCheckoutSessionCommand, Result<CreateCheckoutSessionCommandResult>>
{
    private readonly IPaymentService _paymentService;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCheckoutSessionCommandHandler(IPaymentService paymentService, IUnitOfWork unitOfWork, IDateTimeProvider dateTimeProvider)
    {
        _paymentService = paymentService;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<Result<CreateCheckoutSessionCommandResult>> Handle(CreateCheckoutSessionCommand request, CancellationToken cancellationToken)
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
            PaymentMethod = request.PaymentMethod,
            Amount = purchaseAmount,
            PurchaseTimestamp = _dateTimeProvider.GetCurrentTime(),
        };

        var checkoutSessionUrl = await _paymentService.CreateOneTimeCheckoutSessionAsync(
            GetProductName(order),
            purchaseAmount,
            request.PaymentMethod,
            $"{request.SuccessUrl}/{purchase.Id}",
            $"{request.CancelUrl}/{purchase.Id}"
            );

        await _unitOfWork.PurchaseRepository.CreateAsync(purchase);
        await _unitOfWork.SaveChangesAsync();

        return new CreateCheckoutSessionCommandResult
        {
            CheckoutSessionUrl = checkoutSessionUrl
        };
    }

    private static string GetProductName(Order order) =>
        $"Order number {order.OrderNumber}";
}
