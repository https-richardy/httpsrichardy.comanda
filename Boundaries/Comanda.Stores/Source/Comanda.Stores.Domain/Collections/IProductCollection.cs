namespace Comanda.Stores.Domain.Collections;

public interface IProductCollection : IAggregateCollection<Product>
{
    public Task<IReadOnlyCollection<Product>> FilterProductsAsync(
        ProductFilters filters,
        CancellationToken cancellation = default
    );

    public Task<long> CountProductsAsync(
        ProductFilters filters,
        CancellationToken cancellation = default
    );
}
