namespace Comanda.Subscriptions.Application.Handlers.Subscription;

public sealed class SubscriptionCancelationHandler(ISubscriptionGateway subscriptionGateway) :
    IDispatchHandler<SubscriptionCancelationScheme, Result<SubscriptionScheme>>
{
    public async Task<Result<SubscriptionScheme>> HandleAsync(
        SubscriptionCancelationScheme parameters, CancellationToken cancellation = default)
    {
        var result = await subscriptionGateway.CancelSubscriptionAsync(parameters, cancellation);

        if (result.IsFailure || result.Data is null)
        {
            return Result<SubscriptionScheme>.Failure(result.Error);
        }

        return Result<SubscriptionScheme>.Success(result.Data.AsResponse());
    }
}