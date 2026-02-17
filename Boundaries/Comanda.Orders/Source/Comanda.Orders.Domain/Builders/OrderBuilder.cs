namespace Comanda.Orders.Domain.Builders;

public sealed class OrderBuilder(Order order)
{
    private readonly Order _order = order;

    public OrderBuilder SetCode(Code code)
    {
        _order.Code = code;
        return this;
    }

    public OrderBuilder SetMetadata(Metadata metadata)
    {
        _order.Metadata = metadata;
        return this;
    }

    public OrderBuilder SetStatus(Status status)
    {
        _order.Status = status;
        return this;
    }

    public OrderBuilder SetPriority(Priority priority)
    {
        _order.Priority = priority;
        return this;
    }

    public OrderBuilder SetFulfillment(Fulfillment fulfillment)
    {
        _order.Fulfillment = fulfillment;
        return this;
    }
}
