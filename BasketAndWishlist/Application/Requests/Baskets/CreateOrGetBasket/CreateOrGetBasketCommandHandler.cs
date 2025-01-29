using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Requests.Baskets.CreateOrGetBasket;

// TODO: Tests
public class CreateOrGetBasketCommandHandler : IRequestHandler<CreateOrGetBasketCommand, BasketResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateOrGetBasketCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<BasketResult> Handle(CreateOrGetBasketCommand request, CancellationToken cancellationToken)
    {
        var basket = await GetExistingOrCreateEmptyBasket(request.UserId);

        return _mapper.Map<BasketResult>(basket);
    }

    private async Task<Basket> GetExistingOrCreateEmptyBasket(Guid? userId)
    {
        if (userId.HasValue)
        {
            var existingBasket = await _unitOfWork.BasketRepository.GetByUserIdWithProducts(userId.Value);
            if (existingBasket is not null)
            {
                return existingBasket;
            }
        }

        var createdBasked = new Basket()
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            ProductsInBaskets = []
        };
        await _unitOfWork.BasketRepository.CreateAsync(createdBasked);
        await _unitOfWork.SaveChangesAsync();
        return createdBasked;
    }
}
