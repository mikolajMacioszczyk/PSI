using Application.Interfaces;
using AutoMapper;
using Common.Application.Models;
using MediatR;

namespace Application.Requests.Orders.GetOrderById;

public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, Result<OrderResult>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetOrderByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<OrderResult>> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)

    {
        var order = await _unitOfWork.OrderRepository.GetByIdWithShipment(request.Id);

        if (order is null)
        {
            return new NotFound(request.Id, $"order with provided id {request.Id} does not exists");
        }

        return _mapper.Map<OrderResult>(order);
    }
}
