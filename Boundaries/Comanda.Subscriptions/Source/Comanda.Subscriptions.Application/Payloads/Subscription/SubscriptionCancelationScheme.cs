namespace Comanda.Subscriptions.Application.Payloads.Subscription;

public sealed record SubscriptionCancelationScheme :
    IDispatchable<Result<SubscriptionScheme>>
{
    [property: JsonIgnore]
    public string SubscriptionId { get; init; } = default!;
}