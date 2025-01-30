using Application.Interfaces;
using AutoMapper;
using Common.Application.Interfaces;
using Common.Application.Models;
using MediatR;

namespace Application.Requests.Orders.GetOrderHistory;

public class GetOrderHistoryQueryHandler : IRequestHandler<GetOrderHistoryQuery, Result<PagedResultBase<OrderResult>>>
{
    private readonly IIdentityService _identityService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetOrderHistoryQueryHandler(IIdentityService identityService, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _identityService = identityService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<PagedResultBase<OrderResult>>> Handle(GetOrderHistoryQuery request, CancellationToken cancellationToken)
    {
        var userId = _identityService.UserId;

        var (userOrders, totalCount) = await _unitOfWork.OrderRepository.GetPaged((int)request.PageNumber, (int)request.PageSize, o => o.ClientId == userId, x => x.SubmitionTimestamp);

        var items = _mapper.Map<IEnumerable<OrderResult>>(userOrders);
        return new PagedResultBase<OrderResult>(items.ToList(), (uint)totalCount, request.PageSize, request.PageNumber);
    }
}
