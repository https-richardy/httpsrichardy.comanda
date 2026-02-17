namespace Comanda.Orders.Application.Mappers;

public static class OrderMapper
{
    public static Order AsOrder(this OrderCreationScheme scheme, Code code)
    {
        var order = new Order
        {
            Code = code,
            Fulfillment = scheme.Fulfillment,
            Priority = scheme.Priority,
            Metadata = scheme.Metadata,
            Items = [.. scheme.Items],
        };

        return order;
    }

    public static OrderFilters AsFilters(this OrdersFetchParameters parameters) => new()
    {
        Id = parameters.Id,
        Code = parameters.Code,

        MerchantId = parameters.MerchantId,
        ConsumerId = parameters.ConsumerId,

        Pagination = parameters.Pagination,
        Sort = parameters.Sort,

        CreatedAfter = parameters.CreatedAfter,
        CreatedBefore = parameters.CreatedBefore,

        Status = string.IsNullOrWhiteSpace(parameters.Status)
            ? null
            : Enum.Parse<Status>(parameters.Status, ignoreCase: true),

        Priority = string.IsNullOrWhiteSpace(parameters.Priority)
            ? null
            : Enum.Parse<Priority>(parameters.Priority, ignoreCase: true),

        Fulfillment = string.IsNullOrWhiteSpace(parameters.Fulfillment)
            ? null
            : Enum.Parse<Fulfillment>(parameters.Fulfillment, ignoreCase: true)
    };

    public static OrderScheme AsResponse(this Order order) => new()
    {
        Identifier = order.Id,
        Code = order.Code.Identifier,
        Priority = order.Priority,
        Status = order.Status,
        Fulfillment = order.Fulfillment,
        Items = [.. order.Items],
        Metadata = order.Metadata,
    };
}
