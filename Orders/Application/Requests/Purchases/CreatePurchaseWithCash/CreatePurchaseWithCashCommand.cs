using Common.Application.Models;
using MediatR;

namespace Application.Requests.Purchases.CreatePurchaseWithCash;

public record CreatePurchaseWithCashCommand(Guid OrderId) : IRequest<Result<PurchaseResult>>;
