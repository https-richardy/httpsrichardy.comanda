namespace Comanda.Internal.Contracts.Transport.Internal.Subscriptions;

public sealed record CallbackFailedCheckoutParameters :
    IDispatchable<Result<SubscriptionScheme>>
{
    public string SessionId { get; init; } = default!;
}
