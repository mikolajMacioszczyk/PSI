using Domain.Enums;
using FluentValidation;

namespace Application.Requests.Purchases.CreateCheckoutSession;

public class CreateCheckoutSessionCommandValidator : AbstractValidator<CreateCheckoutSessionCommand>
{
    public CreateCheckoutSessionCommandValidator()
    {
        RuleFor(m => m.PaymentMethod)
            .Must(NotBePaymentByCash)
            .WithMessage("PaymentMethod cannot be payment by cash.");

        RuleFor(m => m.SuccessUrl)
            .Must(BeValidUrl)
            .WithMessage("SuccessUrl must be a valid URL and start with http or https.");

        RuleFor(m => m.CancelUrl)
            .Must(BeValidUrl)
            .WithMessage("CancelUrl must be a valid URL and start with http or https.");
    }

    private bool NotBePaymentByCash(PaymentMethod paymentMethod)
    {
        return paymentMethod != PaymentMethod.CashOnDelivery;
    }

    private bool BeValidUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out var uriResult) &&
               (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }
}
