namespace Comanda.Orders.Domain.Aggregates;

public sealed class Order : Aggregate
{
    public Code Code { get; set; } = default!;
    public Metadata Metadata { get; set; } = default!;
    public Address? Address { get; set; } = default!;

    public Status Status { get; set; } = Status.Pending;
    public Priority Priority { get; set; } = Priority.Normal;
    public Fulfillment Fulfillment { get; set; } = Fulfillment.Unspecified;
    public ICollection<Item> Items { get; set; } = [];

    public void WithChanges(Action<OrderBuilder> action) =>
        action(new OrderBuilder(this));
}
