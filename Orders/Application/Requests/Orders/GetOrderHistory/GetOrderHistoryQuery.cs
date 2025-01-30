using Common.Application.Models;
using MediatR;

namespace Application.Requests.Orders.GetOrderHistory;

// TODO: Implementation
public record GetOrderHistoryQuery : PagedResultQueryBase, IRequest<Result<PagedResultBase<OrderResult>>>;