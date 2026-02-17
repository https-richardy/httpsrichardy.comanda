namespace Comanda.Orchestrator.Infrastructure.Gateways;

public sealed class EstablishmentGateway(IEstablishmentClient establishmentClient, ILogger<IEstablishmentGateway> logger) : IEstablishmentGateway
{
    private readonly AsyncPolicyWrap<Result<PaginationScheme<EstablishmentScheme>>> _establishmentsFetchPolicy =
        PollyPolicies.CreatePolicy<PaginationScheme<EstablishmentScheme>>(logger);

    private readonly AsyncPolicyWrap<Result<EstablishmentScheme>> _establishmentCreationPolicy =
        PollyPolicies.CreatePolicy<EstablishmentScheme>(logger);

    private readonly AsyncPolicyWrap<Result<EstablishmentScheme>> _establishmentModificationPolicy =
        PollyPolicies.CreatePolicy<EstablishmentScheme>(logger);

    private readonly AsyncPolicyWrap<Result> _establishmentDeletionPolicy =
        PollyPolicies.CreatePolicy(logger);

    public async Task<Result<PaginationScheme<EstablishmentScheme>>> GetEstablishmentsAsync(
        EstablishmentsFetchParameters parameters, CancellationToken cancellation = default)
    {
        // applies a full resiliency pattern for external service calls using
        // timeout, retry, fallback, and circuit breaker policies.

        // more details: https://learn.microsoft.com/dotnet/architecture/resilient-applications/
        return await _establishmentsFetchPolicy.ExecuteAsync(token =>
            establishmentClient.GetEstablishmentsAsync(parameters, token), cancellation
        );
    }

    public async Task<Result<EstablishmentScheme>> CreateEstablishmentAsync(
        EstablishmentCreationScheme parameters, CancellationToken cancellation = default)
    {
        // applies a full resiliency pattern for external service calls using
        // timeout, retry, fallback, and circuit breaker policies.

        // more details: https://learn.microsoft.com/dotnet/architecture/resilient-applications/
        return await _establishmentCreationPolicy.ExecuteAsync(token =>
            establishmentClient.CreateEstablishmentAsync(parameters, token), cancellation
        );
    }

    public async Task<Result<EstablishmentScheme>> UpdateEstablishmentAsync(
        EstablishmentModificationScheme parameters, CancellationToken cancellation = default)
    {
        // applies a full resiliency pattern for external service calls using
        // timeout, retry, fallback, and circuit breaker policies.

        // more details: https://learn.microsoft.com/dotnet/architecture/resilient-applications/
        return await _establishmentModificationPolicy.ExecuteAsync(token =>
            establishmentClient.UpdateEstablishmentAsync(parameters, token), cancellation
        );
    }

    public async Task<Result> DeleteEstablishmentAsync(
        EstablishmentDeletionScheme parameters, CancellationToken cancellation = default)
    {
        // applies a full resiliency pattern for external service calls using
        // timeout, retry, fallback, and circuit breaker policies.

        // more details: https://learn.microsoft.com/dotnet/architecture/resilient-applications/
        return await _establishmentDeletionPolicy.ExecuteAsync(token =>
            establishmentClient.DeleteEstablishmentAsync(parameters, token), cancellation
        );
    }
}
