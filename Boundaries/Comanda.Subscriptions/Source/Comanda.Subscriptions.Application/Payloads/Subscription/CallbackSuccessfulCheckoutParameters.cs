namespace Comanda.Subscriptions.Application.Payloads.Subscription;

public sealed record CallbackSuccessfulCheckoutParameters : IDispatchable<Result<SubscriptionScheme>>
{
    public string SessionId { get; init; } = default!;
}