namespace Comanda.Profiles.Application.Validators.Customer;

public sealed class CustomerCreationSchemeValidator : AbstractValidator<CustomerCreationScheme>
{
    public CustomerCreationSchemeValidator()
    {
        RuleFor(customer => customer.FirstName)
            .NotEmpty()
            .WithMessage("First name is required.")
            .Matches(@"^[A-Za-zÀ-ÖØ-öø-ÿ]+$")
            .WithMessage("First name must contain only letters.");

        RuleFor(customer => customer.LastName)
            .NotEmpty()
            .WithMessage("Last name is required.")
            .Matches(@"^[A-Za-zÀ-ÖØ-öø-ÿ]+$")
            .WithMessage("Last name must contain only letters.");

        RuleFor(customer => customer.Email)
            .NotEmpty()
            .WithMessage("Email is required.")
            .Matches(ExpressionPatterns.Email).WithMessage("Email is invalid.");

        RuleFor(customer => customer.PhoneNumber)
            .Matches(ExpressionPatterns.BrazilianPhone)
            .When(customer => !string.IsNullOrWhiteSpace(customer.PhoneNumber))
            .WithMessage("Phone number is invalid.");

        RuleFor(customer => customer.Username)
            .NotEmpty()
            .WithMessage("Username is required.")
            .MinimumLength(3)
            .WithMessage("Username must be at least 3 characters long.");

        RuleFor(customer => customer.UserId)
            .NotEmpty()
            .WithMessage("UserId is required.");
    }
}
