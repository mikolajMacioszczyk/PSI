using Application.Models;
using MediatR;

namespace Application.Requests.Baskets.GetBasketById;

public record GetBasketByIdQuery(Guid Id) : IRequest<Result<BasketResult>>;
