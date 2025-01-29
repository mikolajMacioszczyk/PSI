using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Requests.Baskets.AddProductToBasket;

public class AddProductToBasketCommandHandler : IRequestHandler<AddProductToBasketCommand, BasketResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AddProductToBasketCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<BasketResult> Handle(AddProductToBasketCommand request, CancellationToken cancellationToken)
    {
        var basket = await _unitOfWork.BasketRepository.GetByIdWithProducts(request.BasketId);

        var existingProductEntry = basket!.ProductsInBaskets.FirstOrDefault(p => p.ProductInCatalogId == request.ProductInCatalogId);
        if (existingProductEntry != null)
        {
            existingProductEntry.PieceCount += 1;
            _unitOfWork.ProductInBasketRepository.Update(existingProductEntry);
        }
        else
        {
            var newProductEntry = new ProductInBasket()
            {
                Id = Guid.NewGuid(),
                Basket = basket,
                BasketId = basket.Id,
                ProductInCatalogId = request.ProductInCatalogId,
                PieceCount = 1
            };
            await _unitOfWork.ProductInBasketRepository.CreateAsync(newProductEntry);
        }

        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<BasketResult>(basket);
    }
}
