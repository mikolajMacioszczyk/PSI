using Common.Application.Models;
using MediatR;

namespace Application.Requests.Purchases.CreateCheckoutSession;

public record CreateCheckoutSessionCommand(Guid OrderId, string SuccessUrl, string CancelUrl) : IRequest<Result<CreateCheckoutSessionCommandResult>>;
