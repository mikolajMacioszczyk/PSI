using Common.Application.Models;
using MediatR;

namespace Application.Requests.Orders.GetOrderHistory;

public class GetOrderHistoryQueryHandler : IRequestHandler<GetOrderHistoryQuery, Result<PagedResultBase<OrderResult>>>
{
    public Task<Result<PagedResultBase<OrderResult>>> Handle(GetOrderHistoryQuery request, CancellationToken cancellationToken)
    {
        // TODO
        throw new NotImplementedException();
    }
}
