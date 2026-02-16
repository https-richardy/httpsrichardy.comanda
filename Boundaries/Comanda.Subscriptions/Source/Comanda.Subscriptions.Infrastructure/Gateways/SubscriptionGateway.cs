namespace Comanda.Subscriptions.Infrastructure.Gateways;

public sealed class SubscriptionGateway(ISubscriptionCollection collection) : ISubscriptionGateway
{
    public async Task<Result<CheckoutSession>> CreateCheckoutSessionAsync(
        CheckoutSessionCreationScheme parameters, CancellationToken cancellation = default)
    {
        var sessionService = new SessionService();
        var options = parameters.ToOptions();

        var session = await sessionService.CreateAsync(options, cancellationToken: cancellation);
        var result = new CheckoutSession
        {
            Url = session.Url
        };

        return Result<CheckoutSession>.Success(result);
    }

    public async Task<Result<Subscription>> ProcessSuccessfulCheckoutAsync(
        CallbackSuccessfulCheckoutParameters parameters, CancellationToken cancellation = default)
    {
        var sessionService = new SessionService();
        var session = await sessionService.GetAsync(parameters.SessionId, cancellationToken: cancellation);

        if (session is null)
        {
            return Result<Subscription>.Failure(SubscriptionErrors.SessionDoesNotExist);
        }

        var subscriberId = session.Metadata.GetValueOrDefault("subscriber.identifier");
        var filters = SubscriptionFilters.WithSpecifications()
            .WithSubscriberId(subscriberId)
            .Build();

        var subscriptions = await collection.FilterSubscriptionsAsync(filters, cancellation);
        var subscription = subscriptions.FirstOrDefault();

        if (subscription is null)
        {
            return Result<Subscription>.Failure(SubscriptionErrors.SubscriptionDoesNotExist);
        }

        subscription.Metadata = new SubscriptionMetadata(Identifier: session.SubscriptionId);
        subscription.Status = Status.Active;

        await collection.UpdateAsync(subscription, cancellation);

        return Result<Subscription>.Success(subscription);
    }

    public async Task<Result<Subscription>> ProcessFailedCheckoutAsync(
        CallbackFailedCheckoutParameters parameters, CancellationToken cancellation = default)
    {
        var sessionService = new SessionService();
        var session = await sessionService.GetAsync(parameters.SessionId, cancellationToken: cancellation);

        if (session is null)
        {
            return Result<Subscription>.Failure(SubscriptionErrors.SessionDoesNotExist);
        }

        var subscriberId = session.Metadata.GetValueOrDefault("subscriber.identifier");
        var filters = SubscriptionFilters.WithSpecifications()
            .WithSubscriberId(subscriberId)
            .Build();

        var subscriptions = await collection.FilterSubscriptionsAsync(filters, cancellation);
        var subscription = subscriptions.FirstOrDefault();

        if (subscription is null)
        {
            return Result<Subscription>.Failure(SubscriptionErrors.SubscriptionDoesNotExist);
        }

        subscription.Metadata = SubscriptionMetadata.None;
        subscription.Status = Status.None;

        await collection.UpdateAsync(subscription, cancellation);

        return Result<Subscription>.Success(subscription);
    }

    public async Task<Result<Subscription>> CancelSubscriptionAsync(
        SubscriptionCancelationScheme parameters, CancellationToken cancellation = default)
    {
        var service = new Stripe.SubscriptionService();
        var filters = SubscriptionFilters.WithSpecifications()
            .WithIdentifier(parameters.SubscriptionId)
            .Build();

        var subscriptions = await collection.FilterSubscriptionsAsync(filters, cancellation);
        var subscription = subscriptions.FirstOrDefault();

        if (subscription is null)
        {
            return Result<Subscription>.Failure(SubscriptionErrors.SubscriptionDoesNotExist);
        }

        if (subscription.Status == Status.Canceled)
        {
            return Result<Subscription>.Failure(SubscriptionErrors.SubscriptionAlreadyCanceled);
        }

        if (!string.IsNullOrWhiteSpace(subscription.Metadata.Identifier))
        {
            await service.CancelAsync(subscription.Metadata.Identifier, cancellationToken: cancellation);
        }

        subscription.Status = Status.Canceled;
        subscription.Metadata = SubscriptionMetadata.None;

        await collection.UpdateAsync(subscription, cancellation);

        return Result<Subscription>.Success(subscription);
    }
}
