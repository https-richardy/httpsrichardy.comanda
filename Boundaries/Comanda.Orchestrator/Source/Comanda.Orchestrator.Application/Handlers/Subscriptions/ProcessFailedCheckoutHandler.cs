namespace Comanda.Orchestrator.Application.Handlers.Subscriptions;

public sealed class ProcessFailedCheckoutHandler(ISubscriptionGateway subscriptionGateway) :
    IDispatchHandler<CallbackFailedCheckoutParameters, Result<SubscriptionScheme>>
{
    public async Task<Result<SubscriptionScheme>> HandleAsync(
        CallbackFailedCheckoutParameters parameters, CancellationToken cancellation = default)
    {
        return await subscriptionGateway.ProcessFailedCheckoutAsync(parameters, cancellation);
    }
}
