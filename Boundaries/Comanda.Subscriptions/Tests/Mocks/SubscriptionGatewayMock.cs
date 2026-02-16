namespace Comanda.Subscriptions.TestSuite.Mocks;

public sealed class SubscriptionGatewayMock : ISubscriptionGateway
{
    private readonly Fixture _fixture = new();

    public Task<Result<Subscription>> CancelSubscriptionAsync(SubscriptionCancelationScheme parameters, CancellationToken cancellation = default)
    {
        if (parameters.SubscriptionId == "subscription.canceled")
        {
            return Task.FromResult(Result<Subscription>.Failure(SubscriptionErrors.SubscriptionAlreadyCanceled));
        }

        var subscription = _fixture.Build<Subscription>()
            .With(subscription => subscription.Status, Status.Canceled)
            .With(subscription => subscription.Metadata, SubscriptionMetadata.None)
            .Create();

        return Task.FromResult(Result<Subscription>.Success(subscription));
    }

    public Task<Result<CheckoutSession>> CreateCheckoutSessionAsync(CheckoutSessionCreationScheme parameters, CancellationToken cancellation = default)
    {
        var session = new CheckoutSession
        {
            Url = $"https://checkout.stripe.com/pay/{Guid.NewGuid():N}"
        };

        return Task.FromResult(Result<CheckoutSession>.Success(session));
    }

    public Task<Result<Subscription>> ProcessSuccessfulCheckoutAsync(
        CallbackSuccessfulCheckoutParameters parameters, CancellationToken cancellation = default)
    {
        var subscription = _fixture.Build<Subscription>()
            .With(subscription => subscription.Status, Status.Active)
            .Create();

        return Task.FromResult(Result<Subscription>.Success(subscription));
    }

    public Task<Result<Subscription>> ProcessFailedCheckoutAsync(
        CallbackFailedCheckoutParameters parameters, CancellationToken cancellation = default)
    {
        var subscription = _fixture.Build<Subscription>()
            .With(subscription => subscription.Status, Status.None)
            .With(subscription => subscription.Metadata, SubscriptionMetadata.None)
            .Create();

        return Task.FromResult(Result<Subscription>.Success(subscription));
    }
}
