namespace Comanda.Orchestrator.Application.Handlers.Profiles;

public sealed class EditCustomerAddressHandler(IProfilesGateway profilesGateway) :
    IDispatchHandler<EditCustomerAddressScheme, Result<Address>>
{
    public async Task<Result<Address>> HandleAsync(
        EditCustomerAddressScheme parameters, CancellationToken cancellation = default)
    {
        return await profilesGateway.EditCustomerAddressAsync(parameters, cancellation);
    }
}
