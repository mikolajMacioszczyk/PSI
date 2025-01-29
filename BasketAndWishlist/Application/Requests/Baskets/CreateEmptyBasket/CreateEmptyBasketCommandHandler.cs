using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Requests.Baskets.CreateEmptyBasket;

public class CreateEmptyBasketCommandHandler : IRequestHandler<CreateEmptyBasketCommand, BasketResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateEmptyBasketCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<BasketResult> Handle(CreateEmptyBasketCommand request, CancellationToken cancellationToken)
    {
        var basket = new Basket()
        {
            Id = Guid.NewGuid(),
            // TODO: Validate basket not already exists
            UserId = request.UserId,
            ProductsInBaskets = []
        };

        await _unitOfWork.BasketRepository.CreateAsync(basket);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<BasketResult>(basket);
    }
}
