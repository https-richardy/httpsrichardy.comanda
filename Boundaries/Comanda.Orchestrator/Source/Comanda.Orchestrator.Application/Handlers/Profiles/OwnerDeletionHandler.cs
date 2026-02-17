namespace Comanda.Orchestrator.Application.Handlers.Profiles;

public sealed class OwnerDeletionHandler(IProfilesGateway profilesGateway) :
    IDispatchHandler<OwnerDeletionScheme, Result>
{
    public async Task<Result> HandleAsync(
        OwnerDeletionScheme parameters, CancellationToken cancellation = default)
    {
        return await profilesGateway.DeleteOwnerAsync(parameters, cancellation);
    }
}
