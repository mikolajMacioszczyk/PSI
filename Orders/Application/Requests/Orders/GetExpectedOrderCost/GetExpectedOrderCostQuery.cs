using Common.Application.Models;
using MediatR;

namespace Application.Requests.Orders.GetExpectedOrderCost;

public record GetExpectedOrderCostQuery(Guid BasketId) : IRequest<Result<GetExpectedOrderCostQueryResult>>;
