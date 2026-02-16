namespace Comanda.Internal.Contracts.Clients.Interfaces;

public interface IOwnerClient
{
    public Task<Result<PaginationScheme<OwnerScheme>>> GetOwnersAsync(
        FetchOwnersParameters parameters,
        CancellationToken cancellation = default
    );

    public Task<Result<OwnerScheme>> CreateOwnerAsync(
        OwnerCreationScheme parameters,
        CancellationToken cancellation = default
    );

    public Task<Result<OwnerScheme>> UpdateOwnerAsync(
        EditOwnerScheme parameters,
        CancellationToken cancellation = default
    );

    public Task<Result> DeleteOwnerAsync(
        OwnerDeletionScheme parameters,
        CancellationToken cancellation = default
    );
}
