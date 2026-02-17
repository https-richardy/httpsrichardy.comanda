namespace Comanda.Orchestrator.Application.Handlers.Subscriptions;

public sealed class CancelSubscriptionHandler(ISubscriptionGateway subscriptionGateway) :
    IDispatchHandler<SubscriptionCancelationScheme, Result<SubscriptionScheme>>
{
    public async Task<Result<SubscriptionScheme>> HandleAsync(
        SubscriptionCancelationScheme parameters, CancellationToken cancellation = default)
    {
        return await subscriptionGateway.CancelSubscriptionAsync(parameters, cancellation);
    }
}
