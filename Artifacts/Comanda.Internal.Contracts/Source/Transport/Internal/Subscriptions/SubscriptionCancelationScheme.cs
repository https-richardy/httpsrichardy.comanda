namespace Comanda.Internal.Contracts.Transport.Internal.Subscriptions;

public sealed record SubscriptionCancelationScheme :
    IMessage<Result<SubscriptionScheme>>
{
    [property: JsonIgnore]
    public string SubscriptionId { get; init; } = default!;
}
