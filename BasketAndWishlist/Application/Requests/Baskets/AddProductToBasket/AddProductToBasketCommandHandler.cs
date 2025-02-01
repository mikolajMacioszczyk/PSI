using Application.Interfaces;
using AutoMapper;
using Common.Application.Models;
using Domain.Entities;
using MediatR;

namespace Application.Requests.Baskets.AddProductToBasket;

public class AddProductToBasketCommandHandler : IRequestHandler<AddProductToBasketCommand, Result<BasketResult>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICatalogProductsService _catalogProductsService;
    private readonly IMapper _mapper;

    public AddProductToBasketCommandHandler(IUnitOfWork unitOfWork, ICatalogProductsService catalogProductsService, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _catalogProductsService = catalogProductsService;
        _mapper = mapper;
    }

    public async Task<Result<BasketResult>> Handle(AddProductToBasketCommand request, CancellationToken cancellationToken)
    {
        var basket = await _unitOfWork.BasketRepository.GetByIdWithProducts(request.BasketId);

        if (basket is null)
        {
            return new NotFound(request.BasketId, $"Basket with provided id {request.BasketId} does not exists");
        }

        if (!basket.IsActive)
        {
            return new Failure($"Basket with provided id {request.BasketId} is not active");
        }

        var existingProductEntry = basket.ProductsInBaskets.FirstOrDefault(p => p.ProductInCatalogId == request.ProductInCatalogId);
        if (existingProductEntry != null)
        {
            existingProductEntry.PieceCount += 1;
            _unitOfWork.ProductInBasketRepository.Update(existingProductEntry);
        }
        else
        {
            if (await _catalogProductsService.GetCatalogProductById(request.ProductInCatalogId) is null)
            {
                return new Failure($"Catalog product with provided id {request.ProductInCatalogId} not exists");
            }

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
