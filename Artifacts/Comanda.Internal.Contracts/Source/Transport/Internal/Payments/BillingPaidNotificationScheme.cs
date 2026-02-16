namespace Comanda.Internal.Contracts.Transport.Internal.Payments;

public sealed record BillingPaidNotificationScheme : IEvent
{
    public string Id { get; init; } = default!;
    public string Event { get; init; } = default!;
    public BillingPaidData Data { get; init; } = default!;
}
