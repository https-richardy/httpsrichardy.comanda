namespace Comanda.Orchestrator.Application.Gateways;

public interface ISubscriptionGateway
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
