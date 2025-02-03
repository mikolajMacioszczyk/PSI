using Common.Application.Models;
using MediatR;

namespace Application.Requests.Orders.GetOrderById;

public record GetOrderByIdQuery(Guid Id) : IRequest<Result<GetOrderByIdQueryResult>>;
