using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Requests.Baskets.PopulateMockBasket;

public class PopulateMockBasketCommandHandler : IRequestHandler<PopulateMockBasketCommand, BasketResult>
{
    private static readonly Random random = new ();
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICatalogProductsService _catalogProductsService;
    private readonly IMapper _mapper;

    public PopulateMockBasketCommandHandler(IUnitOfWork unitOfWork, ICatalogProductsService catalogProductsService, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _catalogProductsService = catalogProductsService;
        _mapper = mapper;
    }

    public async Task<BasketResult> Handle(PopulateMockBasketCommand request, CancellationToken cancellationToken)
    {
        var basket = new Basket()
        {
            Id = Guid.NewGuid(),
            ProductsInBaskets = []
        };

        var allActiveCatalogProducts = await _catalogProductsService.GetActiveCatalogProducts();

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
