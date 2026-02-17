namespace Comanda.Orchestrator.Application.Validators.Customer;

public sealed class DeleteCustomerAddressSchemeValidator :
    AbstractValidator<DeleteCustomerAddressScheme>
{
    public DeleteCustomerAddressSchemeValidator()
    {
        RuleFor(payload => payload.Target)
            .NotNull()
            .WithMessage("The target address must be provided for deletion.")
            .SetValidator(new AddressSchemeValidator());
    }
}
