namespace Comanda.Internal.Contracts.Transport.Internal.Subscriptions;

public sealed record SubscriptionScheme
{
    public string Identifier { get; init; } = default!;
    public User Subscriber { get; init; } = default!;
    public Plan Plan { get; init; } = default!;
    public PlanStatus Status { get; init; } = default!;
    public Metadata Metadata { get; init; } = default!;
}
