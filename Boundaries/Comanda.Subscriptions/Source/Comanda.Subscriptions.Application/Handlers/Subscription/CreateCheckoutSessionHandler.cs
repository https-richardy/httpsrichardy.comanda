namespace Comanda.Subscriptions.Application.Handlers.Subscription;

public sealed class CreateCheckoutSessionHandler(ISubscriptionGateway subscriptionGateway, ISubscriptionCollection collection) :
    IDispatchHandler<CheckoutSessionCreationScheme, Result<CheckoutSession>>
{
    public async Task<Result<CheckoutSession>> HandleAsync(
        CheckoutSessionCreationScheme parameters, CancellationToken cancellation = default)
    {
        var subscription = parameters.AsSubscription();
        if (subscription.Plan == Plan.Premium || subscription.Plan == Plan.None)
        {
            /* for tracking purposes: raise error #COMANDA-ERROR-D910F */

            // currently, even though the enum contains other plan types,
            // we only allow the Basic plan. In the future, we will refactor

            // this to allow multiple plans, which will be configurable
            // editable, and manageable through the system

            return Result<CheckoutSession>.Failure(SubscriptionErrors.PlanNotSupported);
        }

        await collection.InsertAsync(subscription, cancellation: cancellation);

        return await subscriptionGateway.CreateCheckoutSessionAsync(parameters, cancellation);
    }
}
