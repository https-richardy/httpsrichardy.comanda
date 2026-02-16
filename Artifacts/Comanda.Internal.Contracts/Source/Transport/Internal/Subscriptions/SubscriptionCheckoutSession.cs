namespace Comanda.Internal.Contracts.Transport.Internal.Subscriptions;

public sealed record SubscriptionCheckoutSession
{
    public string Url { get; init; } = default!;
}
