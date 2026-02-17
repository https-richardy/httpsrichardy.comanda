namespace Comanda.Orchestrator.Application.Validators.Establishments;

public sealed class EstablishmentCreationSchemeValidator : AbstractValidator<EstablishmentCreationScheme>
{
    public EstablishmentCreationSchemeValidator()
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

        RuleFor(establishment => establishment.Owner)
            .NotNull()
            .WithMessage("owner information must be provided.");

        When(establishment => establishment.Owner is not null, () =>
        {
            RuleFor(establishment => establishment.Owner.Identifier)
                .NotEmpty()
                .WithMessage("owner identifier must be provided.");

            RuleFor(establishment => establishment.Owner.Username)
                .NotEmpty()
                .EmailAddress()
                .WithMessage("owner username (email) must be valid.");
        });
    }
}
