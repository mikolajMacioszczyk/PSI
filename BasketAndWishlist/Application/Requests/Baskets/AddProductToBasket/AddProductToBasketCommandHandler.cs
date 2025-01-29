using Application.Interfaces;
using Application.Models;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Requests.Baskets.AddProductToBasket;

// TODO: Tests
public class AddProductToBasketCommandHandler : IRequestHandler<AddProductToBasketCommand, Result<BasketResult>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AddProductToBasketCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<BasketResult>> Handle(AddProductToBasketCommand request, CancellationToken cancellationToken)
    {
        var basket = await _unitOfWork.BasketRepository.GetByIdWithProducts(request.BasketId);

        if (basket is null)
        {
            return new NotFound(request.BasketId, $"Basket with provided id {request.BasketId} does not exists");
        }

        var existingProductEntry = basket.ProductsInBaskets.FirstOrDefault(p => p.ProductInCatalogId == request.ProductInCatalogId);
        if (existingProductEntry != null)
        {
            existingProductEntry.PieceCount += 1;
            _unitOfWork.ProductInBasketRepository.Update(existingProductEntry);
        }
        else
        {
            // TODO: Verify product exists, if not - return failure
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
