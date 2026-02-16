namespace Comanda.Subscriptions.Application.Payloads.Subscription;

public sealed record CallbacksScheme
{
    public string SuccessUrl { get; init; } = default!;
    public string CancelUrl { get; init; } = default!;
}