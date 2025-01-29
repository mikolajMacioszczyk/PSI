using MediatR;

namespace Application.Requests.Orders.CreateOrder;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, OrderResult>
{
    public Task<OrderResult> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        // TODO
        throw new NotImplementedException();
    }
}
