namespace Comanda.Internal.Contracts.Clients.Interfaces;

public interface ISubscriptionClient
{
    public Task<Result<SubscriptionCheckoutSession>> CreateCheckoutSessionAsync(
        SubscriptionCheckoutSessionCreationScheme parameters,
        CancellationToken cancellation = default
    );

    public Task<Result<SubscriptionScheme>> ProcessSuccessfulCheckoutAsync(
        CallbackSuccessfulCheckoutParameters parameters,
        CancellationToken cancellation = default
    );

    public Task<Result<SubscriptionScheme>> ProcessFailedCheckoutAsync(
        CallbackFailedCheckoutParameters parameters,
        CancellationToken cancellation = default
    );

    public Task<Result<SubscriptionScheme>> CancelSubscriptionAsync(
        SubscriptionCancelationScheme parameters,
        CancellationToken cancellation = default
    );
}
