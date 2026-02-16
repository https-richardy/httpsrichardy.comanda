namespace Comanda.Internal.Contracts.Transport.Internal.Subscriptions;

public sealed record SubscriptionCheckoutSessionCreationScheme :
    IDispatchable<Result<SubscriptionCheckoutSession>>
{
    public Plan Plan { get; init; } = Plan.None;
    public User Subscriber { get; init; } = default!;
    public CallbacksScheme Callbacks { get; init; } = default!;
}
