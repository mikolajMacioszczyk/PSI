using Application.Interfaces;
using Common.Application.Models;
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
        //var order = await _unitOfWork.OrderRepository.GetByIdWithShipment(request.OrderId);

        var checkoutSessionUrl = await _paymentService.CreateOneTimeCheckoutSessionAsync(
            "some product",
            10*100,
            "pln",
            "card",
            request.SuccessUrl,
            request.CancelUrl
            );

        return new CreateCheckoutSessionCommandResult
        {
            CheckoutSessionUrl = checkoutSessionUrl
        };
    }
}
