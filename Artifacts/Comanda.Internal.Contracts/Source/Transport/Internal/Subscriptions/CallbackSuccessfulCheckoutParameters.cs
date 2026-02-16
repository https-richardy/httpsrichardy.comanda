namespace Comanda.Internal.Contracts.Transport.Internal.Subscriptions;

public sealed record CallbackSuccessfulCheckoutParameters : IMessage<Result<SubscriptionScheme>>
{
    public string SessionId { get; init; } = default!;
}
