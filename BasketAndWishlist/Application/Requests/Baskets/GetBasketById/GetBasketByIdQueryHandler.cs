using Application.Interfaces;
using Application.Models;
using AutoMapper;
using MediatR;

namespace Application.Requests.Baskets.GetBasketById;

public class GetBasketByIdQueryHandler : IRequestHandler<GetBasketByIdQuery, Result<BasketResult>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetBasketByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<BasketResult>> Handle(GetBasketByIdQuery request, CancellationToken cancellationToken)
    {
        var basket = await _unitOfWork.BasketRepository.GetByIdWithProducts(request.Id);

        if (basket is null)
        {
            return new NotFound(request.Id, $"Basket with provided id {request.Id} does not exists");
        }

        return _mapper.Map<BasketResult>(basket);
    }
}
