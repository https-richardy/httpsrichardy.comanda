namespace Comanda.Orders.Application.Payloads.Order;

public sealed record OrderScheme
{
    public string Identifier { get; init; } = default!;
    public string Code { get; init; } = default!;

    public Address? Address { get; init; } = default!;
    public Status Status { get; init; }
    public Priority Priority { get; init; }
    public Fulfillment Fulfillment { get; init; }
    public IEnumerable<Item> Items { get; init; } = [];
    public Metadata Metadata { get; init; } = default!;
}
