namespace Comanda.Orchestrator.Application.Handlers.Stores;

public sealed class AssignIntegrationCredentialHandler(ICredentialsGateway credentialsGateway) :
    IDispatchHandler<CredentialCreationScheme, Result<CredentialScheme>>
{
    public async Task<Result<CredentialScheme>> HandleAsync(
        CredentialCreationScheme parameters, CancellationToken cancellation = default)
    {
        return await credentialsGateway.AssignIntegrationCredentialAsync(parameters, cancellation);
    }
}
