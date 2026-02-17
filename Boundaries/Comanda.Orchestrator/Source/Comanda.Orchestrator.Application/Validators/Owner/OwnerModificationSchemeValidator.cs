namespace Comanda.Orchestrator.Application.Validators.Owner;

public sealed class OwnerModificationSchemeValidator : AbstractValidator<EditOwnerScheme>
{
    public OwnerModificationSchemeValidator()
    {
        RuleFor(owner => owner.FirstName)
            .NotEmpty()
            .WithMessage("the first name is required.")
            .Matches(@"^[A-Za-zÀ-ÖØ-öø-ÿ]+$")
            .WithMessage("the first name must contain only letters.");

        RuleFor(owner => owner.LastName)
            .NotEmpty()
            .WithMessage("the last name is required.")
            .Matches(@"^[A-Za-zÀ-ÖØ-öø-ÿ]+$")
            .WithMessage("the last name must contain only letters.");

        RuleFor(owner => owner.Email)
            .NotEmpty()
            .WithMessage("the email is required.")
            .Matches(ExpressionPatterns.Email)
            .WithMessage("the email is invalid.");

        RuleFor(owner => owner.PhoneNumber)
            .Matches(ExpressionPatterns.BrazilianPhone)
            .When(owner => !string.IsNullOrWhiteSpace(owner.PhoneNumber))
            .WithMessage("the phone number is invalid.");
    }
}
