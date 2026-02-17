namespace Comanda.Orders.Domain.Filtering.Builders;

public sealed class OrderFiltersBuilder :
    FiltersBuilderBase<OrderFilters, OrderFiltersBuilder>
{
    public static OrderFiltersBuilder WithSpecifications()
    {
        return new OrderFiltersBuilder();
    }

    public OrderFiltersBuilder WithCode(string? code)
    {
        _filters.Code = code;
        return this;
    }

    public OrderFiltersBuilder WithMerchantId(string? merchantId)
    {
        _filters.MerchantId = merchantId;
        return this;
    }

    public OrderFiltersBuilder WithConsumerId(string? consumerId)
    {
        _filters.ConsumerId = consumerId;
        return this;
    }

    public OrderFiltersBuilder WithStatus(Status? status)
    {
        _filters.Status = status;
        return this;
    }

    public OrderFiltersBuilder WithPriority(Priority? priority)
    {
        _filters.Priority = priority;
        return this;
    }

    public OrderFiltersBuilder WithFulfillment(Fulfillment? fulfillment)
    {
        _filters.Fulfillment = fulfillment;
        return this;
    }
}
