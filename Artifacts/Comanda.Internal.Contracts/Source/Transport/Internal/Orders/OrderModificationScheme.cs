namespace Comanda.Internal.Contracts.Transport.Internal.Orders;

public sealed record OrderModificationScheme : IDispatchable<Result<OrderScheme>>
{
    [property: JsonIgnore]
    public string Id { get; init; } = default!;

    public Status Status { get; init; }
    public Priority Priority { get; init; }
}
