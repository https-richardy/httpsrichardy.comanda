namespace Comanda.Stores.Domain.Collections;

public interface IEstablishmentCollection : IAggregateCollection<Establishment>
{
    public Task<IReadOnlyCollection<Establishment>> FilterEstablishmentsAsync(
        EstablishmentFilters filters,
        CancellationToken cancellation = default
    );

    public Task<long> CountEstablishmentsAsync(
        EstablishmentFilters filters,
        CancellationToken cancellation = default
    );
}
