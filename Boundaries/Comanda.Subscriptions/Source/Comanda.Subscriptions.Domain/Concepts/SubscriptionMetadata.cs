namespace Comanda.Subscriptions.Domain.Concepts;

public sealed record SubscriptionMetadata(string Identifier) :
    IValueObject<SubscriptionMetadata>
{
    public static SubscriptionMetadata None =>
        new(string.Empty);
}