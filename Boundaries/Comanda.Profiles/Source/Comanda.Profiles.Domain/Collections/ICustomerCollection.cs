namespace Comanda.Profiles.Domain.Collections;

public interface ICustomerCollection : IAggregateCollection<Customer>
{
    public Task<IReadOnlyCollection<Customer>> GetCustomersAsync(
        CustomerFilters filters,
        CancellationToken cancellation = default
    );

    public Task<long> CountCustomersAsync(
        CustomerFilters filters,
        CancellationToken cancellation = default
    );
}
