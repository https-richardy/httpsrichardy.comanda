namespace Comanda.Orchestrator.Infrastructure.Gateways;

public sealed class SubscriptionGateway(ISubscriptionClient subscriptionClient, ILogger<ISubscriptionGateway> logger) : ISubscriptionGateway
{
    private readonly AsyncPolicyWrap<Result<SubscriptionCheckoutSession>> _checkoutSessionPolicy =
        PollyPolicies.CreatePolicy<SubscriptionCheckoutSession>(logger);

    private readonly AsyncPolicyWrap<Result<SubscriptionScheme>> _processSuccessfulCheckoutPolicy =
        PollyPolicies.CreatePolicy<SubscriptionScheme>(logger);

    private readonly AsyncPolicyWrap<Result<SubscriptionScheme>> _processFailedCheckoutPolicy =
        PollyPolicies.CreatePolicy<SubscriptionScheme>(logger);

    private readonly AsyncPolicyWrap<Result<SubscriptionScheme>> _cancelSubscriptionPolicy =
        PollyPolicies.CreatePolicy<SubscriptionScheme>(logger);

    public async Task<Result<SubscriptionCheckoutSession>> CreateCheckoutSessionAsync(
        SubscriptionCheckoutSessionCreationScheme parameters,
        CancellationToken cancellation = default)
    {
        // applies a full resiliency pattern for external service calls using
        // timeout, retry, fallback, and circuit breaker policies.

        // more details: https://learn.microsoft.com/dotnet/architecture/resilient-applications/
        return await _checkoutSessionPolicy.ExecuteAsync(token =>
            subscriptionClient.CreateCheckoutSessionAsync(parameters, token), cancellation
        );
    }

    public async Task<Result<SubscriptionScheme>> ProcessSuccessfulCheckoutAsync(
        CallbackSuccessfulCheckoutParameters parameters,
        CancellationToken cancellation = default)
    {
        // applies a full resiliency pattern for external service calls using
        // timeout, retry, fallback, and circuit breaker policies.

        // more details: https://learn.microsoft.com/dotnet/architecture/resilient-applications/
        return await _processSuccessfulCheckoutPolicy.ExecuteAsync(token =>
            subscriptionClient.ProcessSuccessfulCheckoutAsync(parameters, token), cancellation
        );
    }

    public async Task<Result<SubscriptionScheme>> ProcessFailedCheckoutAsync(
        CallbackFailedCheckoutParameters parameters,
        CancellationToken cancellation = default)
    {
        // applies a full resiliency pattern for external service calls using
        // timeout, retry, fallback, and circuit breaker policies.

        // more details: https://learn.microsoft.com/dotnet/architecture/resilient-applications/
        return await _processFailedCheckoutPolicy.ExecuteAsync(token =>
            subscriptionClient.ProcessFailedCheckoutAsync(parameters, token), cancellation
        );
    }

    public async Task<Result<SubscriptionScheme>> CancelSubscriptionAsync(
        SubscriptionCancelationScheme parameters,
        CancellationToken cancellation = default)
    {
        // applies a full resiliency pattern for external service calls using
        // timeout, retry, fallback, and circuit breaker policies.

        // more details: https://learn.microsoft.com/dotnet/architecture/resilient-applications/
        return await _cancelSubscriptionPolicy.ExecuteAsync(token =>
            subscriptionClient.CancelSubscriptionAsync(parameters, token), cancellation
        );
    }
}
