namespace Comanda.Stores.Application.Validators.Establishment;

public sealed class CredentialEditSchemeValidator :
    AbstractValidator<CredentialEditScheme>
{
    public CredentialEditSchemeValidator()
    {
        RuleFor(credential => credential.Provider)
            .IsInEnum()
            .WithMessage("provider must be a valid integration target.");

        RuleFor(credential => credential.SecretKey)
            .NotEmpty()
            .WithMessage("secret key must be provided.")
            .MaximumLength(500)
            .WithMessage("secret key must not exceed 500 characters.");
    }
}
