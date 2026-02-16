namespace Comanda.Profiles.Application.Validators.Customer;

public sealed class AddressSchemeValidator : AbstractValidator<Address>
{
    public AddressSchemeValidator()
    {
        RuleFor(address => address.Street)
            .NotEmpty()
            .WithMessage("The street field is required and cannot be empty.");

        RuleFor(address => address.Number)
            .NotEmpty()
            .WithMessage("The number field is required and represents the street number of the address.");

        RuleFor(address => address.City)
            .NotEmpty()
            .WithMessage("The city field is required and cannot be empty.");

        RuleFor(address => address.State)
            .NotEmpty()
            .WithMessage("The state field is required and cannot be empty.");

        RuleFor(address => address.ZipCode)
            .NotEmpty()
            .WithMessage("The zip code field is required.")
            .Matches(ExpressionPatterns.Cep)
            .WithMessage("The zip code is invalid. It must match the Brazilian postal code format.");
    }
}
