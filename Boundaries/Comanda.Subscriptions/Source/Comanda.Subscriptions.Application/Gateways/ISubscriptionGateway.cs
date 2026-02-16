namespace Comanda.Subscriptions.Application.Gateways;

public interface ISubscriptionGateway
{
    public Task<Result<CheckoutSession>> CreateCheckoutSessionAsync(
        CheckoutSessionCreationScheme parameters,
        CancellationToken cancellation = default
    );

    public Task<Result<Subscription>> ProcessSuccessfulCheckoutAsync(
        CallbackSuccessfulCheckoutParameters parameters,
        CancellationToken cancellation = default
    );

    public Task<Result<Subscription>> ProcessFailedCheckoutAsync(
        CallbackFailedCheckoutParameters parameters,
        CancellationToken cancellation = default
    );

    public Task<Result<Subscription>> CancelSubscriptionAsync(
        SubscriptionCancelationScheme parameters,
        CancellationToken cancellation = default
    );
}
