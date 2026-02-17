namespace Comanda.Orders.Application.Handlers.Order;

public sealed class OrdersFetchHandler(IOrderCollection repository) :
    IDispatchHandler<OrdersFetchParameters, Result<PaginationScheme<OrderScheme>>>
{
    public async Task<Result<PaginationScheme<OrderScheme>>> HandleAsync(
        OrdersFetchParameters parameters, CancellationToken cancellation = default)
    {
        var filters = parameters.AsFilters();

        var orders = await repository.GetOrdersAsync(filters, cancellation);
        var total = await repository.CountOrdersAsync(filters, cancellation);

        var pagination = new PaginationScheme<OrderScheme>
        {
            Items = [.. orders.Select(order => order.AsResponse())],
            Total = (int)total,

            PageSize = parameters.Pagination?.PageSize ?? 1,
            PageNumber = parameters.Pagination?.PageNumber ?? 20
        };

        return Result<PaginationScheme<OrderScheme>>.Success(pagination);
    }
}
