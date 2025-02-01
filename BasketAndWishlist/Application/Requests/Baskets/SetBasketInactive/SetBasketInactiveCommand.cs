using Common.Application.Models;
using MediatR;

namespace Application.Requests.Baskets.SetBasketInactive;

public record SetBasketInactiveCommand(Guid BasketId) : IRequest<Result<BasketResult>>;
