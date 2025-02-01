using MediatR;

namespace Application.Requests.Baskets.CreateOrGetBasket;

public record CreateOrGetBasketCommand() : IRequest<BasketResult>;
