using Common.Application.Models;
using MediatR;

namespace Application.Requests.Orders.CreateOrder;

// TODO: Fill
public record CreateOrderCommand(Guid BasketId) : IRequest<Result<OrderResult>>;
