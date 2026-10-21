namespace Comanda.Internal.Contracts.Clients.Interfaces;

public interface IClientsClient
{
    public Task<Result<Pagination<ClientScheme>>> GetClientsAsync(
        ClientFiltersParameters parameters,
        CancellationToken cancellation = default
    );

    public Task<Result<ClientScheme>> GetClientAsync(
        string id,
        CancellationToken cancellation = default
    );
}
