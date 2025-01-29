using Application.Interfaces;
using Common.Application.Models;
using MediatR;

namespace Application.Requests.Orders.CreateOrder;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Result<OrderResult>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBasketService _basketService;

    public CreateOrderCommandHandler(IUnitOfWork unitOfWork, IBasketService basketService)
    {
        _unitOfWork = unitOfWork;
        _basketService = basketService;
    }

    public Task<Result<OrderResult>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        // TODO
        throw new NotImplementedException();
    }
}
