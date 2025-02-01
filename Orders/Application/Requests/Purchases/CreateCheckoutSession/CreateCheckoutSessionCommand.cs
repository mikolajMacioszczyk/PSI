using Common.Application.Models;
using Domain.Enums;
using MediatR;

namespace Application.Requests.Purchases.CreateCheckoutSession;

public record CreateCheckoutSessionCommand(Guid OrderId, PaymentMethod PaymentMethod, string SuccessUrl, string CancelUrl) : IRequest<Result<CreateCheckoutSessionCommandResult>>;
