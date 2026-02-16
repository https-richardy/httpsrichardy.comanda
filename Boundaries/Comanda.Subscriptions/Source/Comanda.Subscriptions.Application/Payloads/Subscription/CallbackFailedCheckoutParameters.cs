namespace Comanda.Subscriptions.Application.Payloads.Subscription;

public sealed record CallbackFailedCheckoutParameters : IDispatchable<Result<SubscriptionScheme>>
{
    public string SessionId { get; init; } = default!;
}
