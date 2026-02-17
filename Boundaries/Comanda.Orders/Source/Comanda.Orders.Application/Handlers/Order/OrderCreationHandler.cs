namespace Comanda.Orders.Application.Handlers.Order;

public sealed class OrderCreationHandler(IOrderCollection repository, IOrderService service) :
    IDispatchHandler<OrderCreationScheme, Result<OrderScheme>>
{
    public async Task<Result<OrderScheme>> HandleAsync(
        OrderCreationScheme parameters, CancellationToken cancellation = default)
    {
        var code = await service.GenerateCodeAsync(cancellation);
        var order = await repository.InsertAsync(parameters.AsOrder(code), cancellation: cancellation);

        return Result<OrderScheme>.Success(order.AsResponse());
    }
}
