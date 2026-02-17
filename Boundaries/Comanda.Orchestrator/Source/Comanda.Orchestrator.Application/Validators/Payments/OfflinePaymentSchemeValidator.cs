namespace Comanda.Orchestrator.Application.Validators.Payments;

public sealed class OfflinePaymentSchemeValidator : AbstractValidator<OfflinePaymentScheme>
{
    public OfflinePaymentSchemeValidator()
    {
        RuleFor(payment => payment.Amount)
            .GreaterThan(0)
            .WithMessage("the amount must be greater than zero.");

        RuleFor(payment => payment.Reference)
            .NotEmpty()
            .MaximumLength(140)
            .WithMessage("the reference must be provided and contain up to 140 characters.");

        RuleFor(payment => payment.Method)
            .NotEqual(Method.Unspecified)
            .WithMessage("payment method must be specified.");
    }
}
