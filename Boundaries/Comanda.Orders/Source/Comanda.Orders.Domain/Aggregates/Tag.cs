namespace Comanda.Orders.Domain.Aggregates;

public sealed class Tag : Aggregate
{
    public Code Code { get; set; } = default!;
    public Metadata Metadata { get; set; } = default!;
}
