using FluentValidation;
using System.Text.RegularExpressions;

namespace Application.Requests.Orders.CreateOrder;

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(c => c.ConsentGranted)
            .Must(x => x == true)
            .WithMessage("Consents must be granted");

        RuleFor(c => c.Email)
           .Must(BeValidEmail)
           .WithMessage("Enter a valid email address.");
    }

    private bool BeValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        // Simple regex pattern for email validation
        var emailRegex = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(email, emailRegex);
    }
}
