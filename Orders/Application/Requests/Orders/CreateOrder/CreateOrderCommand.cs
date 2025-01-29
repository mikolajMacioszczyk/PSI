using MediatR;

namespace Application.Requests.Orders.CreateOrder;

// TODO: Fill
// TODO: Result
public record CreateOrderCommand(Guid BasketId) : IRequest<OrderResult>;
