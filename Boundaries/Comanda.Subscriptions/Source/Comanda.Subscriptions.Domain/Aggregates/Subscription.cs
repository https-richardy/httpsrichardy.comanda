namespace Comanda.Subscriptions.Domain.Aggregates;

public sealed class Subscription : Aggregate
{
    public Plan Plan { get; set; } = Plan.None;
    public Status Status { get; set; } = Status.None;
    public User Subscriber { get; set; } = default!;
    public SubscriptionMetadata Metadata { get; set; } = SubscriptionMetadata.None;
}
