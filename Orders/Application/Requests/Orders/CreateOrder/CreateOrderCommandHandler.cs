using Common.Application.Models;
using MediatR;

namespace Application.Requests.Orders.CreateOrder;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Result<OrderResult>>
{
    public Task<Result<OrderResult>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        // TODO
        throw new NotImplementedException();
    }
}
