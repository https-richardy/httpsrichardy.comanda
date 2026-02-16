namespace Comanda.Internal.Contracts.Transport.Internal.Subscriptions;

public sealed record CallbackSuccessfulCheckoutParameters : IDispatchable<Result<SubscriptionScheme>>
{
    public string SessionId { get; init; } = default!;
}
