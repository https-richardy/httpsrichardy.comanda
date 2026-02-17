namespace Comanda.Orchestrator.Application.Handlers.Stores;

public sealed class CredentialModificationHandler(ICredentialsGateway credentialsGateway) :
    IDispatchHandler<CredentialModificationScheme, Result<CredentialScheme>>
{
    public async Task<Result<CredentialScheme>> HandleAsync(
        CredentialModificationScheme parameters, CancellationToken cancellation = default)
    {
        return await credentialsGateway.UpdateCredentialAsync(parameters, cancellation);
    }
}
