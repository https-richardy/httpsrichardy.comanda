namespace Comanda.Orchestrator.Application.Handlers.Orders;

public sealed class OrdersFetchHandler(IOrdersGateway ordersGateway) :
    IDispatchHandler<OrdersFetchParameters, Result<PaginationScheme<OrderScheme>>>
{
    public async Task<Result<PaginationScheme<OrderScheme>>> HandleAsync(
        OrdersFetchParameters parameters, CancellationToken cancellation = default)
    {
        return await ordersGateway.GetOrdersAsync(parameters, cancellation);
    }
}
