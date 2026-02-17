namespace Comanda.Subscriptions.Application.Handlers.Subscription;

public sealed class ProcessFailedCheckoutHandler(ISubscriptionGateway subscriptionGateway) :
    IDispatchHandler<CallbackFailedCheckoutParameters, Result<SubscriptionScheme>>
{
    public async Task<Result<SubscriptionScheme>> HandleAsync(
        CallbackFailedCheckoutParameters parameters, CancellationToken cancellation = default)
    {
        var result = await subscriptionGateway.ProcessFailedCheckoutAsync(parameters, cancellation);

        if (result.IsFailure || result.Data is null)
        {
            return Result<SubscriptionScheme>.Failure(result.Error);
        }

        return Result<SubscriptionScheme>.Success(result.Data.AsResponse());
    }
}