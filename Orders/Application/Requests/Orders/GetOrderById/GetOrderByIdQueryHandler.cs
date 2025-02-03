using Application.Interfaces;
using AutoMapper;
using Common.Application.Models;
using MediatR;

namespace Application.Requests.Orders.GetOrderById;

public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, Result<GetOrderByIdQueryResult>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBasketService _basketService;
    private readonly ICatalogService _catalogService;
    private readonly IMapper _mapper;

    public GetOrderByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IBasketService basketService, ICatalogService catalogService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _basketService = basketService;
        _catalogService = catalogService;
    }

    public async Task<Result<GetOrderByIdQueryResult>> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)

    {
        var order = await _unitOfWork.OrderRepository.GetByIdWithShipmentAndPurchase(request.Id);

        if (order is null)
        {
            return new NotFound(request.Id, $"order with provided id {request.Id} does not exists");
        }

        var orderResult = _mapper.Map<GetOrderByIdQueryResult>(order);

        var basket = await _basketService.GetBasketById(order.BasketId);
        var catalogIds = basket!.ProductsInBaskets.Select(p => p.ProductInCatalogId).Distinct().ToList();

        orderResult.Products = await _catalogService.GetCatalogProductsByIds(catalogIds);

        return orderResult;
    }
}
