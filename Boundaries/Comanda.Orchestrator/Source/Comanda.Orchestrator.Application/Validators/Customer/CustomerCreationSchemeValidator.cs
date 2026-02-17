namespace Comanda.Orchestrator.Application.Validators.Customer;

public sealed class CustomerCreationSchemeValidator : AbstractValidator<CustomerCreationScheme>
{
    public CustomerCreationSchemeValidator()
    {
        RuleFor(customer => customer.FirstName)
            .NotEmpty()
            .WithMessage("the first name is required.")
            .Matches(@"^[A-Za-zÀ-ÖØ-öø-ÿ]+$")
            .WithMessage("the first name must contain only letters.");

        RuleFor(customer => customer.LastName)
            .NotEmpty()
            .WithMessage("the last name is required.")
            .Matches(@"^[A-Za-zÀ-ÖØ-öø-ÿ]+$")
            .WithMessage("the last name must contain only letters.");

        RuleFor(customer => customer.Email)
            .NotEmpty()
            .WithMessage("the email is required.")
            .Matches(ExpressionPatterns.Email)
            .WithMessage("the email is invalid.");

        RuleFor(customer => customer.PhoneNumber)
            .Matches(ExpressionPatterns.BrazilianPhone)
            .When(customer => !string.IsNullOrWhiteSpace(customer.PhoneNumber))
            .WithMessage("the phone number is invalid.");

        RuleFor(customer => customer.Username)
            .NotEmpty()
            .WithMessage("the username is required.")
            .MinimumLength(3)
            .WithMessage("the username must be at least 3 characters long.");

        RuleFor(customer => customer.UserId)
            .NotEmpty()
            .WithMessage("the userId field is required.");
    }
}
