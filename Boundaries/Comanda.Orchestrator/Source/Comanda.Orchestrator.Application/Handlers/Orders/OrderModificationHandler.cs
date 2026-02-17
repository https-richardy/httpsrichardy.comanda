namespace Comanda.Orchestrator.Application.Handlers.Orders;

public sealed class OrderModificatioHandler(IOrdersGateway ordersGateway) :
    IDispatchHandler<OrderModificationScheme, Result<OrderScheme>>
{
    public async Task<Result<OrderScheme>> HandleAsync(
        OrderModificationScheme parameters, CancellationToken cancellation = default)
    {
        return await ordersGateway.UpdateOrderAsync(parameters, cancellation);
    }
}
