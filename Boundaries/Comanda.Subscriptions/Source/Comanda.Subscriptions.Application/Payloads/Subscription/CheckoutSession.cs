namespace Comanda.Subscriptions.Application.Payloads.Subscription;

public sealed record CheckoutSession
{
    public string Url { get; init; } = default!;
}