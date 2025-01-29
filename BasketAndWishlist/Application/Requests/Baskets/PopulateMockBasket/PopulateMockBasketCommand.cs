using MediatR;

namespace Application.Requests.Baskets.PopulateMockBasket
{
    public record PopulateMockBasketCommand(uint productsCount) : IRequest<BasketResult>;
}
