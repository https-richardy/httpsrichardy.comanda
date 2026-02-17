namespace Comanda.Orchestrator.Application.Handlers.Subscriptions;

public sealed class CreateCheckoutSessionHandler(ISubscriptionGateway subscriptionGateway) :
    IDispatchHandler<SubscriptionCheckoutSessionCreationScheme, Result<SubscriptionCheckoutSession>>
{
    public async Task<Result<SubscriptionCheckoutSession>> HandleAsync(
        SubscriptionCheckoutSessionCreationScheme parameters, CancellationToken cancellation = default)
    {
        return await subscriptionGateway.CreateCheckoutSessionAsync(parameters, cancellation);
    }
}
