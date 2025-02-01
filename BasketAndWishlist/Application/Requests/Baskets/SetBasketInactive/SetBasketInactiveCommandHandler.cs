using Application.Interfaces;
using AutoMapper;
using Common.Application.Models;
using MediatR;

namespace Application.Requests.Baskets.SetBasketInactive;

public class SetBasketInactiveCommandHandler : IRequestHandler<SetBasketInactiveCommand, Result<BasketResult>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public SetBasketInactiveCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<BasketResult>> Handle(SetBasketInactiveCommand request, CancellationToken cancellationToken)
    {
        var basket = await _unitOfWork.BasketRepository.GetById(request.BasketId);

        if (basket is null)
        {
            return new NotFound(request.BasketId, $"Basket with provided id {request.BasketId} does not exists");
        }

        if (!basket.IsActive)
        {
            return new Failure($"Basket with provided id {request.BasketId} is already inactive");
        }

        basket.IsActive = false;
        _unitOfWork.BasketRepository.Update(basket);

        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<BasketResult>(basket);
    }
}
