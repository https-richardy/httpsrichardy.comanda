namespace Comanda.Subscriptions.Application.Handlers.Subscription;

public sealed class ProcessSuccessfulCheckoutHandler(ISubscriptionGateway subscriptionGateway) :
    IDispatchHandler<CallbackSuccessfulCheckoutParameters, Result<SubscriptionScheme>>
{
    public async Task<Result<SubscriptionScheme>> HandleAsync(
        CallbackSuccessfulCheckoutParameters parameters, CancellationToken cancellation = default)
    {
        var result = await subscriptionGateway.ProcessSuccessfulCheckoutAsync(parameters, cancellation);

        if (result.IsFailure || result.Data is null)
        {
            return Result<SubscriptionScheme>.Failure(result.Error);
        }

        return Result<SubscriptionScheme>.Success(result.Data.AsResponse());
    }
}