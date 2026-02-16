namespace Comanda.Internal.Contracts.Transport.Internal.Orders;

public sealed record OrderModificationScheme : IMessage<Result<OrderScheme>>
{
    [property: JsonIgnore]
    public string Id { get; init; } = default!;

    public Status Status { get; init; }
    public Priority Priority { get; init; }
}
