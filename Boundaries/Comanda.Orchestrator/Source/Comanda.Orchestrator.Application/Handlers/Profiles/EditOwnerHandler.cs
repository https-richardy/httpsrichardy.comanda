namespace Comanda.Orchestrator.Application.Handlers.Profiles;

public sealed class EditOwnerHandler(IProfilesGateway profilesGateway) :
    IDispatchHandler<EditOwnerScheme, Result<OwnerScheme>>
{
    public async Task<Result<OwnerScheme>> HandleAsync(
        EditOwnerScheme parameters, CancellationToken cancellation = default)
    {
        return await profilesGateway.UpdateOwnerAsync(parameters, cancellation);
    }
}
