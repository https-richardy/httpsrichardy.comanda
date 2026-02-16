namespace Comanda.Subscriptions.Application.Mappers;

public static class SubscriptionMapper
{
    public static Subscription AsSubscription(this CheckoutSessionCreationScheme scheme) => new()
    {
        Plan = scheme.Plan,
        Subscriber = scheme.Subscriber,
        Metadata = SubscriptionMetadata.None
    };

    public static SubscriptionScheme AsResponse(this Subscription subscription) => new()
    {
        Identifier = subscription.Id,
        Plan = subscription.Plan,
        Subscriber = subscription.Subscriber,
        Status = subscription.Status,
        Metadata = subscription.Metadata
    };
}
