using Common.Application.Models;
using MediatR;

namespace Application.Requests.Purchases.GetPurchaseById;

public record GetPurchaseByIdQuery(Guid PurchaseId) : IRequest<Result<PurchaseResult>>;
