namespace Comanda.Orchestrator.Application.Handlers.Orders;

public sealed class OrderCreationHandler(IOrdersGateway ordersGateway) :
    IDispatchHandler<OrderCreationScheme, Result<OrderScheme>>
{
    public async Task<Result<OrderScheme>> HandleAsync(
        OrderCreationScheme parameters, CancellationToken cancellation = default)
    {
        return await ordersGateway.CreateOrderAsync(parameters, cancellation);
    }
}
