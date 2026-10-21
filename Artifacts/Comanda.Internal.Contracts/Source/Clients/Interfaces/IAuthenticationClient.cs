namespace Comanda.Internal.Contracts.Clients.Interfaces;

public interface IAuthenticationClient
{
    public Task<Result<AuthenticationScheme>> AuthenticateAsync(
        ClientCredentials credentials,
        CancellationToken cancellation = default
    );
}
