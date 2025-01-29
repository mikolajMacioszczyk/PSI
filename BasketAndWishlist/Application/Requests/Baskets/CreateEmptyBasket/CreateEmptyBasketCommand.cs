using MediatR;

namespace Application.Requests.Baskets.CreateEmptyBasket;

public record CreateEmptyBasketCommand(Guid? UserId) : IRequest<BasketResult>;
