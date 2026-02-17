namespace Comanda.Orders.Domain.Collections;

public interface IOrderCollection : IAggregateCollection<Order>
{
    public Task<IReadOnlyCollection<Order>> GetOrdersAsync(
        OrderFilters filters,
        CancellationToken cancellation = default
    );

    public Task<long> CountOrdersAsync(
        OrderFilters filters,
        CancellationToken cancellation = default
    );
}
