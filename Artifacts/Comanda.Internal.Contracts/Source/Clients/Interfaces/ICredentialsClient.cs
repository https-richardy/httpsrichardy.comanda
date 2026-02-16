namespace Comanda.Internal.Contracts.Clients.Interfaces;

public interface ICredentialsClient
{
    public Task<Result<IReadOnlyCollection<CredentialScheme>>> GetCredentialsAsync(
        CredentialsFetchParameters parameters,
        CancellationToken cancellation = default
    );

    public Task<Result<CredentialScheme>> AssignIntegrationCredentialAsync(
        CredentialCreationScheme parameters,
        CancellationToken cancellation = default
    );

    public Task<Result<CredentialScheme>> UpdateCredentialAsync(
        CredentialModificationScheme parameters,
        CancellationToken cancellation = default
    );
}
