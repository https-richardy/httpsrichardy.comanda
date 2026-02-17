namespace Comanda.Orchestrator.Application.Validators.Establishments;

public sealed class EstablishmentModificationSchemeValidator : AbstractValidator<EstablishmentModificationScheme>
{
    public EstablishmentModificationSchemeValidator()
    {
        RuleFor(establishment => establishment.Title)
            .NotEmpty()
            .WithMessage("establishment title must be provided.")
            .MaximumLength(100)
            .WithMessage("establishment title must not exceed 100 characters.");

        RuleFor(establishment => establishment.Description)
            .NotEmpty()
            .WithMessage("establishment description must be provided.")
            .MaximumLength(500)
            .WithMessage("establishment description must not exceed 500 characters.");

        When(establishment => establishment.Branding is not null, () =>
        {
            RuleFor(establishment => establishment.Branding.PrimaryColor)
                .NotEmpty()
                .WithMessage("primary color must be provided.")
                .Matches("^#(?:[0-9a-fA-F]{3}){1,2}$")
                .WithMessage("primary color must be a valid hex color.");

            RuleFor(establishment => establishment.Branding.SecondaryColor)
                .NotEmpty()
                .WithMessage("secondary color must be provided.")
                .Matches("^#(?:[0-9a-fA-F]{3}){1,2}$")
                .WithMessage("secondary color must be a valid hex color.");

            RuleFor(establishment => establishment.Branding.Logo)
                .NotEmpty()
                .WithMessage("logo must be provided.")
                .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
                .WithMessage("logo must be a valid URL.");
        });
    }
}
