namespace Comanda.Payments.Application.Validators;

public sealed class CheckoutSessionCreationSchemeValidator :
    AbstractValidator<CheckoutSessionCreationScheme>
{
    public CheckoutSessionCreationSchemeValidator()
    {
        RuleFor(session => session.Amount)
            .GreaterThan(0)
            .WithMessage("the amount must be greater than zero.");

        RuleFor(session => session.Reference)
            .NotEmpty()
            .MaximumLength(140)
            .WithMessage("the reference must be provided and contain up to 140 characters.");

        RuleFor(session => session.Payer)
            .NotNull()
            .WithMessage("payer information must be provided.");

        When(session => session.Payer is not null, () =>
        {
            RuleFor(session => session.Payer.Identifier)
                .NotEmpty()
                .WithMessage("payer identifier must be provided.");

            RuleFor(session => session.Payer.Username)
                .NotEmpty()
                .EmailAddress()
                .WithMessage("payer email must be valid.");
        });
    }
}
