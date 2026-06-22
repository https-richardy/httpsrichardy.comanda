namespace Comanda.Orders.Domain.Collections;

public interface ITagCollection : IAggregateCollection<Tag>
{
    public Task<IReadOnlyCollection<Tag>> FilterTagsAsync(
        TagFilters filters,
        CancellationToken cancellation = default
    );

    public Task<System.Numerics.BigInteger> CountTagsAsync(
        TagFilters filters,
        CancellationToken cancellation = default
    );
}
