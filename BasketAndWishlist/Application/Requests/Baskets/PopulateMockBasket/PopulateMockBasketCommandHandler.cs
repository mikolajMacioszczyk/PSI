using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Requests.Baskets.PopulateMockBasket;

public class PopulateMockBasketCommandHandler : IRequestHandler<PopulateMockBasketCommand, BasketResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public PopulateMockBasketCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<BasketResult> Handle(PopulateMockBasketCommand request, CancellationToken cancellationToken)
    {
        var basket = new Basket()
        {
            Id = Guid.NewGuid(),
        };

        // TODO: get all products

        // TODO: create random basket wint n products

        await _unitOfWork.BasketRepository.CreateAsync(basket);

        await _unitOfWork.SaveChangesAsync();

        // TODO: Mapping
        throw new NotImplementedException();
    }
}
