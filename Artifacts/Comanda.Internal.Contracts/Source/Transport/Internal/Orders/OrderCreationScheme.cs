namespace Comanda.Internal.Contracts.Transport.Internal.Orders;

public sealed record OrderCreationScheme : IDispatchable<Result<OrderScheme>>
{
    public Fulfillment Fulfillment { get; init; }
    public Priority Priority { get; init; }
    public Address? Address { get; init; } = default!;
    public Metadata Metadata { get; init; } = default!;
    public IEnumerable<Item> Items { get; init; } = [];
}
