using Application.Interfaces;
using AutoMapper;
using Common.Application.Models;
using MediatR;

namespace Application.Requests.Purchases.GetPurchaseById;

public class GetPurchaseByIdQueryHandler : IRequestHandler<GetPurchaseByIdQuery, Result<PurchaseResult>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetPurchaseByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<PurchaseResult>> Handle(GetPurchaseByIdQuery request, CancellationToken cancellationToken)
    {
        var purchase = await _unitOfWork.PurchaseRepository.GetById(request.PurchaseId);
        if (purchase is null)
        {
            return new NotFound(request.PurchaseId, $"purchase with provided id {request.PurchaseId} not exists");
        }

        return _mapper.Map<PurchaseResult>(purchase);
    }
}
