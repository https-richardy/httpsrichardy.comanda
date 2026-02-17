namespace Comanda.Orchestrator.Application.Handlers.Subscriptions;

public sealed class ProcessSuccessfulCheckoutHandler(ISubscriptionGateway subscriptionGateway) :
    IDispatchHandler<CallbackSuccessfulCheckoutParameters, Result<SubscriptionScheme>>
{
    public async Task<Result<SubscriptionScheme>> HandleAsync(
        CallbackSuccessfulCheckoutParameters parameters, CancellationToken cancellation = default)
    {
        return await subscriptionGateway.ProcessSuccessfulCheckoutAsync(parameters, cancellation);
    }
}
