using Application.Interfaces;
using Stripe.Checkout;

namespace Infrastructure.PaymentsAdapter;

public class StripePaymentService : IPaymentService
{
    /// <summary>
    /// Creates checkout session
    /// </summary>
    /// <param name="productName">Only a label</param>
    /// <param name="amount">Amount in cents (e.g., $10.00 = 1000)</param>
    /// <param name="currency">like pln</param>
    /// <param name="currency">like pln</param>
    /// <param name="paymentMethodType">One of: "card", "blik", "p24", "paypal", </param>
    /// <param name="cancelUrl"></param>
    /// <returns></returns>
    public async Task<string> CreateOneTimeCheckoutSessionAsync(
        string productName, 
        long amount, 
        string currency, 
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
                        UnitAmount = amount,
                        Currency = currency,
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = productName
                        }
                    },
                    Quantity = 1
                }
            ],
            Mode = "payment",
            SuccessUrl = successUrl,
            CancelUrl = cancelUrl
        };

        var service = new SessionService();
        Session session = await service.CreateAsync(options);
        return session.Url;
    }
}
