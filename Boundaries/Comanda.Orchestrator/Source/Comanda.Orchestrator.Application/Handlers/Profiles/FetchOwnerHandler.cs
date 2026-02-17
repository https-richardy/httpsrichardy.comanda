namespace Comanda.Orchestrator.Application.Handlers.Profiles;

public sealed class FetchOwnerHandler(IProfilesGateway profilesGateway) :
    IDispatchHandler<FetchOwnersParameters, Result<PaginationScheme<OwnerScheme>>>
{
    public async Task<Result<PaginationScheme<OwnerScheme>>> HandleAsync(
        FetchOwnersParameters parameters, CancellationToken cancellation = default)
    {
        return await profilesGateway.GetOwnersAsync(parameters, cancellation);
    }
}
