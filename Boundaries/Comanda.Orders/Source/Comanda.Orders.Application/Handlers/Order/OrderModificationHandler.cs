namespace Comanda.Orders.Application.Handlers.Order;

public sealed class OrderModificationHandler(IOrderCollection repository) :
    IDispatchHandler<OrderModificationScheme, Result<OrderScheme>>
{
    public async Task<Result<OrderScheme>> HandleAsync(
        OrderModificationScheme parameters, CancellationToken cancellation = default)
    {
        var filters = OrderFilters.WithSpecifications()
            .WithIdentifier(parameters.Id)
            .Build();

        var orders = await repository.GetOrdersAsync(filters, cancellation);
        var existingOrder = orders.FirstOrDefault();

        if (existingOrder is null)
        {
            return Result<OrderScheme>.Failure(OrderErrors.OrderDoesNotExist);
        }

        existingOrder.WithChanges(builder =>
        {
            builder.SetStatus(parameters.Status);
            builder.SetPriority(parameters.Priority);
        });

        var order = await repository.UpdateAsync(existingOrder, cancellation);

        return Result<OrderScheme>.Success(order.AsResponse());
    }
}
