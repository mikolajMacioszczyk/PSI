namespace Application.Interfaces;

public interface IPaymentService
{
    Task<string> CreateOneTimeCheckoutSessionAsync(
        string productName,
        long amount,
        string currency,
        string paymentMethodType,
        string successUrl,
        string cancelUrl);
}
