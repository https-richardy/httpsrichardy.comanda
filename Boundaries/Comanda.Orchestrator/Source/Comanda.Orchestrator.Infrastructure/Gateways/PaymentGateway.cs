namespace Comanda.Orchestrator.Infrastructure.Gateways;

public sealed class PaymentGateway(IPaymentClient paymentClient, ILogger<IPaymentGateway> logger) : IPaymentGateway
{
    private readonly AsyncPolicyWrap<Result<PaymentScheme>> _localPaymentPolicy =
        PollyPolicies.CreatePolicy<PaymentScheme>(logger);

    private readonly AsyncPolicyWrap<Result<CheckoutSession>> _onlinePaymentPolicy =
        PollyPolicies.CreatePolicy<CheckoutSession>(logger);

    private readonly AsyncPolicyWrap<Result<PaginationScheme<PaymentScheme>>> _paymentsFetchPolicy =
        PollyPolicies.CreatePolicy<PaginationScheme<PaymentScheme>>(logger);

    private readonly AsyncPolicyWrap<Result<PaymentScheme>> _paymentMutationPolicy =
        PollyPolicies.CreatePolicy<PaymentScheme>(logger);

    public async Task<Result<PaymentScheme>> CreateLocalPaymentAsync(
        OfflinePaymentScheme parameters, CancellationToken cancellation = default)
    {
        // applies a full resiliency pattern for external service calls using
        // timeout, retry, fallback, and circuit breaker policies.

        // more details: https://learn.microsoft.com/dotnet/architecture/resilient-applications/
        return await _localPaymentPolicy.ExecuteAsync(token =>
            paymentClient.CreateLocalPaymentAsync(parameters, token), cancellation
        );
    }

    public async Task<Result<CheckoutSession>> CreateOnlineChargeAsync(
        CheckoutSessionCreationScheme parameters, Credential credential, CancellationToken cancellation = default)
    {
        // applies a full resiliency pattern for external service calls using
        // timeout, retry, fallback, and circuit breaker policies.

        // more details: https://learn.microsoft.com/dotnet/architecture/resilient-applications/
        return await _onlinePaymentPolicy.ExecuteAsync(token =>
            paymentClient.CreateOnlineChargeAsync(parameters, credential, token), cancellation
        );
    }

    public async Task<Result<PaginationScheme<PaymentScheme>>> GetPaymentsAsync(
        PaymentsFetchParameters parameters, CancellationToken cancellation = default)
    {
        // applies a full resiliency pattern for external service calls using
        // timeout, retry, fallback, and circuit breaker policies.

        // more details: https://learn.microsoft.com/dotnet/architecture/resilient-applications/
        return await _paymentsFetchPolicy.ExecuteAsync(token =>
            paymentClient.GetPaymentsAsync(parameters, token), cancellation
        );
    }

    public async Task<Result<PaymentScheme>> UpdatePaymentStatusAsync(
        PaymentStatusUpdateScheme parameters, CancellationToken cancellation = default)
    {
        // applies a full resiliency pattern for external service calls using
        // timeout, retry, fallback, and circuit breaker policies.

        // more details: https://learn.microsoft.com/dotnet/architecture/resilient-applications/
        return await _paymentMutationPolicy.ExecuteAsync(token =>
            paymentClient.UpdatePaymentStatusAsync(parameters, token), cancellation
        );
    }
}
