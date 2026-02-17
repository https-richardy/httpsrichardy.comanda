namespace Comanda.Orchestrator.Application.Handlers.Stores;

public sealed class CredentialsFetchHandler(ICredentialsGateway credentialsGateway) :
    IDispatchHandler<CredentialsFetchParameters, Result<IEnumerable<CredentialScheme>>>
{
    public async Task<Result<IEnumerable<CredentialScheme>>> HandleAsync(
        CredentialsFetchParameters parameters, CancellationToken cancellation = default)
    {
        var credentials = await credentialsGateway.GetCredentialsAsync(parameters, cancellation);
        if (credentials.IsFailure || credentials.Data is null)
        {
            return Result<IEnumerable<CredentialScheme>>.Failure(credentials.Error);
        }

        return Result<IEnumerable<CredentialScheme>>.Success([.. credentials.Data]);
    }
}
