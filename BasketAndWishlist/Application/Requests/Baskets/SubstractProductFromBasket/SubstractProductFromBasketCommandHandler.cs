using Application.Interfaces;
using AutoMapper;
using MediatR;

namespace Application.Requests.Baskets.SubstractProductFromBasket;
public class SubstractProductFromBasketCommandHandler : IRequestHandler<SubstractProductFromBasketCommand, BasketResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public SubstractProductFromBasketCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<BasketResult> Handle(SubstractProductFromBasketCommand request, CancellationToken cancellationToken)
    {
        var basket = await _unitOfWork.BasketRepository.GetByIdWithProducts(request.BasketId);

        var existingProductEntry = basket!.ProductsInBaskets.FirstOrDefault(p => p.ProductInCatalogId == request.ProductInCatalogId);
        if (existingProductEntry is not null)
        {
            existingProductEntry.PieceCount -= 1;

            if (existingProductEntry.PieceCount > 0)
            {
                _unitOfWork.ProductInBasketRepository.Update(existingProductEntry);
            }
            else
            {
                _unitOfWork.ProductInBasketRepository.Remove(existingProductEntry);
            }
        }

        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<BasketResult>(basket);
    }
}