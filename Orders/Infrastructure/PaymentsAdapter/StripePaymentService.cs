using Application.Interfaces;
using Domain.Enums;
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
        PaymentMethod paymentMethod,
        string successUrl, 
        string cancelUrl)
    {
        var options = new SessionCreateOptions
        {
            PaymentMethodTypes = ConvertPaymentMethodToStripeFormat(paymentMethod),
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

    private static List<string> ConvertPaymentMethodToStripeFormat(PaymentMethod paymentMethod) => paymentMethod switch
    {
        PaymentMethod.ElectronicWallet => ["paypal"], // Electronic wallets
        PaymentMethod.PaymentCard => ["card"], // Credit/Debit Cards
        PaymentMethod.GooglePay => ["card"], // Google Pay is supported via Stripe Checkout when "card" is enabled
        PaymentMethod.TraditionalTransfer => ["p24"], // Przelewy24
        PaymentMethod.Blik => ["blik"], 
        _ => throw new ArgumentOutOfRangeException(nameof(paymentMethod), $"Unsupported payment method: {paymentMethod}")
    };

    private static long ConvertAmountToLong(decimal amount) =>
        (long) (amount * 100);
}
