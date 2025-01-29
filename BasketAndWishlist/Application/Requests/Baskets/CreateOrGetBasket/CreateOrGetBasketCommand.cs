using MediatR;

namespace Application.Requests.Baskets.CreateOrGetBasket;

public record CreateOrGetBasketCommand(Guid? UserId) : IRequest<BasketResult>;
