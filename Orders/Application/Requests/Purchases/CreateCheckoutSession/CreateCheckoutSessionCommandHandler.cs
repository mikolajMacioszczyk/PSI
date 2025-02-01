using Application.Interfaces;
using Common.Application.Models;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.Requests.Purchases.CreateCheckoutSession;

public class CreateCheckoutSessionCommandHandler : IRequestHandler<CreateCheckoutSessionCommand, Result<CreateCheckoutSessionCommandResult>>
{
    private readonly IPaymentService _paymentService;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCheckoutSessionCommandHandler(IPaymentService paymentService, IUnitOfWork unitOfWork)
    {
        _paymentService = paymentService;
        _unitOfWork = unitOfWork;
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

        var checkoutSessionUrl = await _paymentService.CreateOneTimeCheckoutSessionAsync(
            GetProductName(order),
            order.OrderPrice + order.Shipment.ShipmentPrice,
            // TODO: Payment method
            "card",
            request.SuccessUrl,
            request.CancelUrl
            );

        return new CreateCheckoutSessionCommandResult
        {
            CheckoutSessionUrl = checkoutSessionUrl
        };
    }

    private static string GetProductName(Order order) =>
        $"Order number {order.OrderNumber}";
}
