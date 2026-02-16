namespace Comanda.Subscriptions.Application.Payloads.Subscription;

public sealed record CheckoutSessionCreationScheme : IDispatchable<Result<CheckoutSession>>
{
    public Plan Plan { get; init; } = Plan.None;
    public User Subscriber { get; init; } = default!;
    public CallbacksScheme Callbacks { get; init; } = default!;
}
