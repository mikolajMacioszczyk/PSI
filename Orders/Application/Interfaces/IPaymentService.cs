namespace Application.Interfaces;

public interface IPaymentService
{
    Task<string> CreateOneTimeCheckoutSessionAsync(
        string productName,
        decimal amount,
        string paymentMethodType,
        string successUrl,
        string cancelUrl);
}
