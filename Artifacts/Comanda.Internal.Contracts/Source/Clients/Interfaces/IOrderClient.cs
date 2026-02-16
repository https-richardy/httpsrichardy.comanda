namespace Comanda.Internal.Contracts.Clients.Interfaces;

public interface IOrderClient
{
    public Task<Result<PaginationScheme<OrderScheme>>> GetOrdersAsync(
        OrdersFetchParameters parameters,
        CancellationToken cancellation = default
    );

    public Task<Result<OrderScheme>> CreateOrderAsync(
        OrderCreationScheme parameters,
        CancellationToken cancellation = default
    );

    public Task<Result<OrderScheme>> UpdateOrderAsync(
        OrderModificationScheme parameters,
        CancellationToken cancellation = default
    );
}
