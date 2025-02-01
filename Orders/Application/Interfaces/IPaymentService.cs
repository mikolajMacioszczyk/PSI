using Domain.Enums;

namespace Application.Interfaces;

public interface IPaymentService
{
    Task<string> CreateOneTimeCheckoutSessionAsync(
        string productName,
        decimal amount,
        PaymentMethod paymentMethod,
        string successUrl,
        string cancelUrl);
}
