namespace Comanda.Subscriptions.Application.Validators.Subscription;

public sealed class CheckoutSessionCreationSchemeValidator :
    AbstractValidator<CheckoutSessionCreationScheme>
{
    public CheckoutSessionCreationSchemeValidator()
    {
        RuleFor(scheme => scheme.Plan)
            .NotEqual(Plan.None)
            .WithMessage("plan must be specified");

        RuleFor(scheme => scheme.Subscriber)
            .NotNull()
            .WithMessage("subscriber must be provided");

        RuleFor(scheme => scheme.Callbacks)
            .NotNull()
            .WithMessage("callbacks must be provided");

        RuleFor(scheme => scheme.Callbacks.SuccessUrl)
            .NotEmpty()
            .WithMessage("successUrl must be a valid URL");

        RuleFor(scheme => scheme.Callbacks.CancelUrl)
            .NotEmpty()
            .WithMessage("cancelUrl must be a valid URL");
    }
}
