namespace Comanda.Profiles.Application.Validators.Customer;

public sealed class EditCustomerAddressSchemeValidator : AbstractValidator<EditCustomerAddressScheme>
{
    public EditCustomerAddressSchemeValidator()
    {
        RuleFor(payload => payload.Target)
            .NotNull()
            .WithMessage("The target address must be provided for editing.")
            .SetValidator(new AddressSchemeValidator());

        RuleFor(payload => payload.Replacement)
            .NotNull()
            .WithMessage("The replacement address must be provided for editing.")
            .SetValidator(new AddressSchemeValidator());

        When(payload => payload.Target is not null && payload.Replacement is not null, () =>
        {
            RuleFor(payload => payload.Replacement)
                .Must((payload, replacement) => !AreAddressesEqual(payload.Target, replacement))
                .WithMessage("The replacement address must be different from the target address.");
        });
    }

    private static bool AreAddressesEqual(Address target, Address replacement)
    {
        if (target is null || replacement is null) return false;

        static bool StringsEqual(string? first, string? second) =>
            string.Equals(first ?? string.Empty, second ?? string.Empty, StringComparison.OrdinalIgnoreCase);

        return StringsEqual(target.Street, replacement.Street)
            && StringsEqual(target.Number, replacement.Number)
            && StringsEqual(target.City, replacement.City)
            && StringsEqual(target.State, replacement.State)
            && StringsEqual(target.ZipCode, replacement.ZipCode)
            && StringsEqual(target.Neighborhood, replacement.Neighborhood)
            && StringsEqual(target.Complement, replacement.Complement)
            && StringsEqual(target.Reference, replacement.Reference);
    }
}
