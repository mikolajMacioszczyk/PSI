using Application.Interfaces;
using AutoMapper;
using MediatR;

namespace Application.Requests.Baskets.GetBasketById;

public class GetBasketByIdQueryHandler : IRequestHandler<GetBasketByIdQuery, BasketResult?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetBasketByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<BasketResult?> Handle(GetBasketByIdQuery request, CancellationToken cancellationToken)
    {
        var basket = await _unitOfWork.BasketRepository.GetByIdWithProducts(request.Id);
        return _mapper.Map<BasketResult>(basket);
    }
}
