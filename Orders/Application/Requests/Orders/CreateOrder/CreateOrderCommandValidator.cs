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

        RuleFor(c => c.FirstName)
            .MaximumLength(128)
            .WithMessage("FirstName must be a string with a maximum length of 128 characters");

        RuleFor(c => c.LastName)
           .MaximumLength(128)
           .WithMessage("LastName must be a string with a maximum length of 128 characters");

        RuleFor(c => c.LastName)
           .MaximumLength(128)
           .WithMessage("LastName must be a string with a maximum length of 128 characters");

        RuleFor(c => c.Email)
           .Must(BeValidEmail)
           .WithMessage("Enter a valid email address.")
           .MaximumLength(512)
           .WithMessage("Email must be a string with a maximum length of 512 characters");

        RuleFor(c => c.Country)
           .MaximumLength(128)
           .WithMessage("Country must be a string with a maximum length of 128 characters");

        RuleFor(c => c.Street)
           .MaximumLength(128)
           .WithMessage("Street must be a string with a maximum length of 128 characters");

        RuleFor(c => c.PostalCode)
          .MaximumLength(6)
          .WithMessage("PostalCode must be a string with a maximum length of 6 characters");

        RuleFor(c => c.PhoneNumber)
          .MaximumLength(15)
          .WithMessage("PhoneNumber must be a string with a maximum length of 15 characters");

        RuleFor(c => c.AreaCode)
          .MaximumLength(3)
          .WithMessage("AreaCode must be a string with a maximum length of 3 characters");
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
