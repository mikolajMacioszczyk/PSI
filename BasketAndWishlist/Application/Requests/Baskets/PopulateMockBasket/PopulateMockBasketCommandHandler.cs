using Application.Interfaces;
using AutoMapper;
using Common.Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Requests.Baskets.PopulateMockBasket;

public class PopulateMockBasketCommandHandler : IRequestHandler<PopulateMockBasketCommand, BasketResult>
{
    private static readonly Random random = new ();
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICatalogProductsService _catalogProductsService;
    private readonly IIdentityService _identityService;
    private readonly IMapper _mapper;

    public PopulateMockBasketCommandHandler(
        IUnitOfWork unitOfWork, 
        ICatalogProductsService catalogProductsService, 
        IMapper mapper, 
        IIdentityService identityService)
    {
        _unitOfWork = unitOfWork;
        _catalogProductsService = catalogProductsService;
        _mapper = mapper;
        _identityService = identityService;
    }

    public async Task<BasketResult> Handle(PopulateMockBasketCommand request, CancellationToken cancellationToken)
    {
        var basket = new Basket()
        {
            Id = Guid.NewGuid(),
            UserId = _identityService.UserId,
            ProductsInBaskets = [],
            IsActive = true,
        };

        var allActiveCatalogProducts = await _catalogProductsService.GetActiveCatalogProducts(pageSize: 100, pageNumber: 1);

        var randomProducts = allActiveCatalogProducts.OrderBy(x => random.Next()).Take((int)request.ProductsCount).ToList();

        foreach (var catalogProduct in randomProducts)
        {
            var productInBasket = new ProductInBasket()
            {
                Id = Guid.NewGuid(),
                Basket = basket,
                BasketId = basket.Id,
                ProductInCatalogId = catalogProduct.Id,
                PieceCount = random.Next(1, 5)
            };
            basket.ProductsInBaskets.Add(productInBasket);
        }

        await _unitOfWork.BasketRepository.CreateAsync(basket);

        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<BasketResult>(basket);
    }
}
