namespace Comanda.Profiles.Application.Validators.Owner;

public sealed class EditOwnerSchemeValidator : AbstractValidator<EditOwnerScheme>
{
    public EditOwnerSchemeValidator()
    {
        RuleFor(owner => owner.FirstName)
            .NotEmpty()
            .WithMessage("First name is required.")
            .Matches(@"^[A-Za-zÀ-ÖØ-öø-ÿ]+$")
            .WithMessage("First name must contain only letters.");

        RuleFor(owner => owner.LastName)
            .NotEmpty()
            .WithMessage("Last name is required.")
            .Matches(@"^[A-Za-zÀ-ÖØ-öø-ÿ]+$")
            .WithMessage("Last name must contain only letters.");

        RuleFor(owner => owner.Email)
            .NotEmpty()
            .WithMessage("Email is required.")
            .Matches(ExpressionPatterns.Email)
            .WithMessage("Email is invalid.");

        RuleFor(owner => owner.PhoneNumber)
            .Matches(ExpressionPatterns.BrazilianPhone)
            .When(owner => !string.IsNullOrWhiteSpace(owner.PhoneNumber))
            .WithMessage("Phone number is invalid.");
    }
}
