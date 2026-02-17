namespace Comanda.Orchestrator.Infrastructure.Gateways;

public sealed class OrdersGateway(IOrderClient orderClient, ILogger<IOrdersGateway> logger) : IOrdersGateway
{
    private readonly AsyncPolicyWrap<Result<PaginationScheme<OrderScheme>>> _ordersFetchPolicy =
        PollyPolicies.CreatePolicy<PaginationScheme<OrderScheme>>(logger);

    private readonly AsyncPolicyWrap<Result<OrderScheme>> _orderCreationPolicy =
        PollyPolicies.CreatePolicy<OrderScheme>(logger);

    private readonly AsyncPolicyWrap<Result<OrderScheme>> _orderModificationPolicy =
        PollyPolicies.CreatePolicy<OrderScheme>(logger);

    public async Task<Result<PaginationScheme<OrderScheme>>> GetOrdersAsync(
        OrdersFetchParameters parameters, CancellationToken cancellation = default)
    {
        // applies a full resiliency pattern for external service calls using
        // timeout, retry, fallback, and circuit breaker policies.

        // more details: https://learn.microsoft.com/dotnet/architecture/resilient-applications/
        return await _ordersFetchPolicy.ExecuteAsync(token =>
            orderClient.GetOrdersAsync(parameters, token), cancellation
        );
    }

    public async Task<Result<OrderScheme>> CreateOrderAsync(
        OrderCreationScheme parameters, CancellationToken cancellation = default)
    {
        // applies a full resiliency pattern for external service calls using
        // timeout, retry, fallback, and circuit breaker policies.

        // more details: https://learn.microsoft.com/dotnet/architecture/resilient-applications/
        return await _orderCreationPolicy.ExecuteAsync(token =>
            orderClient.CreateOrderAsync(parameters, token), cancellation
        );
    }

    public async Task<Result<OrderScheme>> UpdateOrderAsync(
        OrderModificationScheme parameters, CancellationToken cancellation = default)
    {
        // applies a full resiliency pattern for external service calls using
        // timeout, retry, fallback, and circuit breaker policies.

        // more details: https://learn.microsoft.com/dotnet/architecture/resilient-applications/
        return await _orderModificationPolicy.ExecuteAsync(token =>
            orderClient.UpdateOrderAsync(parameters, token), cancellation
        );
    }
}
