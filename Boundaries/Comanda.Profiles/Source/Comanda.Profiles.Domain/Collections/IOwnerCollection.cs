namespace Comanda.Profiles.Domain.Collections;

public interface IOwnerCollection : IAggregateCollection<Owner>
{
    public Task<IReadOnlyCollection<Owner>> GetOwnersAsync(
        OwnerFilters filters,
        CancellationToken cancellation = default
    );

    public Task<long> CountOwnersAsync(
        OwnerFilters filters,
        CancellationToken cancellation = default
    );
}
