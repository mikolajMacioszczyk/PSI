using Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Stripe.Checkout;

namespace Infrastructure.PaymentsAdapter;

public class StripePaymentService : IPaymentService
{
    private const string paymentMode = "payment";
    private readonly string _currency;

    public StripePaymentService(IConfiguration configuration)
    {
        _currency = configuration.GetSection("Payment").GetValue<string>("Currency")
            ?? throw new NullReferenceException("Currency");
    }

    public async Task<string> CreateOneTimeCheckoutSessionAsync(
        string productName, 
        decimal amount, 
        string paymentMethodType,
        string successUrl, 
        string cancelUrl)
    {
        var options = new SessionCreateOptions
        {
            PaymentMethodTypes = [paymentMethodType],
            LineItems =
            [
                new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = ConvertAmountToLong(amount),
                        Currency = _currency,
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = productName
                        }
                    },
                    Quantity = 1
                }
            ],
            Mode = paymentMode,
            SuccessUrl = successUrl,
            CancelUrl = cancelUrl
        };

        var service = new SessionService();
        Session session = await service.CreateAsync(options);
        return session.Url;
    }

    private static long ConvertAmountToLong(decimal amount) =>
        (long) (amount * 100);
}
