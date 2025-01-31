using Application.Interfaces;
using AutoMapper;
using Common.Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Requests.Baskets.CreateOrGetBasket;
public class CreateOrGetBasketCommandHandler : IRequestHandler<CreateOrGetBasketCommand, BasketResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IIdentityService _identityService;
    private readonly IMapper _mapper;

    public CreateOrGetBasketCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IIdentityService identityService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _identityService = identityService;
    }

    public async Task<BasketResult> Handle(CreateOrGetBasketCommand request, CancellationToken cancellationToken)
    {
        var basket = await GetExistingOrCreateEmptyBasket();

        return _mapper.Map<BasketResult>(basket);
    }

    private async Task<Basket> GetExistingOrCreateEmptyBasket()
    {
        Guid? userId = _identityService.TryGetUserId();
        if (userId.HasValue)
        {
            var existingBasket = await _unitOfWork.BasketRepository.GetByUserIdWithProducts(userId.Value);
            if (existingBasket is not null && existingBasket.IsActive)
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
