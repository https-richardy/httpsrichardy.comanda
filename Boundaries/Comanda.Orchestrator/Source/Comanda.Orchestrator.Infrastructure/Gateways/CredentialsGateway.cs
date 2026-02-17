namespace Comanda.Orchestrator.Infrastructure.Gateways;

public sealed class CredentialsGateway(ICredentialsClient credentialsClient, ILogger<ICredentialsGateway> logger) : ICredentialsGateway
{
    private readonly AsyncPolicyWrap<Result<IReadOnlyCollection<CredentialScheme>>> _credentialsFetchPolicy =
        PollyPolicies.CreatePolicy<IReadOnlyCollection<CredentialScheme>>(logger);

    private readonly AsyncPolicyWrap<Result<CredentialScheme>> _credentialMutationPolicy =
        PollyPolicies.CreatePolicy<CredentialScheme>(logger);

    public async Task<Result<IReadOnlyCollection<CredentialScheme>>> GetCredentialsAsync(
        CredentialsFetchParameters parameters, CancellationToken cancellation = default)
    {
        // applies a full resiliency pattern for external service calls using
        // timeout, retry, fallback, and circuit breaker policies.

        // more details: https://learn.microsoft.com/dotnet/architecture/resilient-applications/
        return await _credentialsFetchPolicy.ExecuteAsync(token =>
            credentialsClient.GetCredentialsAsync(parameters, token), cancellation
        );
    }

    public async Task<Result<CredentialScheme>> AssignIntegrationCredentialAsync(
        CredentialCreationScheme parameters, CancellationToken cancellation = default)
    {
        // applies a full resiliency pattern for external service calls using
        // timeout, retry, fallback, and circuit breaker policies.

        // more details: https://learn.microsoft.com/dotnet/architecture/resilient-applications/
        return await _credentialMutationPolicy.ExecuteAsync(token =>
            credentialsClient.AssignIntegrationCredentialAsync(parameters, token), cancellation
        );
    }

    public async Task<Result<CredentialScheme>> UpdateCredentialAsync(
        CredentialModificationScheme parameters, CancellationToken cancellation = default)
    {
        // applies a full resiliency pattern for external service calls using
        // timeout, retry, fallback, and circuit breaker policies.

        // more details: https://learn.microsoft.com/dotnet/architecture/resilient-applications/
        return await _credentialMutationPolicy.ExecuteAsync(token =>
            credentialsClient.UpdateCredentialAsync(parameters, token), cancellation
        );
    }
}
