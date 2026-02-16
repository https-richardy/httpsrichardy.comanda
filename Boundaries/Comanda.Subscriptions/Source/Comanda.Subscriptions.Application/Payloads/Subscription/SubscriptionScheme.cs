namespace Comanda.Subscriptions.Application.Payloads.Subscription;

public sealed record SubscriptionScheme
{
    public string Identifier { get; init; } = default!;
    public User Subscriber { get; init; } = default!;
    public Plan Plan { get; init; } = default!;
    public Status Status { get; init; } = default!;
    public SubscriptionMetadata Metadata { get; init; } = default!;
}
