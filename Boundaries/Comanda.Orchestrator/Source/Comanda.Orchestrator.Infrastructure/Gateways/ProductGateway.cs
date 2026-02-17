namespace Comanda.Orchestrator.Infrastructure.Gateways;

public sealed class ProductGateway(IProductClient productClient, ILogger<IProductGateway> logger) : IProductGateway
{
    private readonly AsyncPolicyWrap<Result<PaginationScheme<ProductScheme>>> _productsFetchPolicy =
        PollyPolicies.CreatePolicy<PaginationScheme<ProductScheme>>(logger);

    private readonly AsyncPolicyWrap<Result<ProductScheme>> _productCreationPolicy =
        PollyPolicies.CreatePolicy<ProductScheme>(logger);

    private readonly AsyncPolicyWrap<Result<ProductScheme>> _productModificationPolicy =
        PollyPolicies.CreatePolicy<ProductScheme>(logger);

    private readonly AsyncPolicyWrap<Result> _productDeletionPolicy =
        PollyPolicies.CreatePolicy(logger);

    private readonly AsyncPolicyWrap<Result> _productImageUploadPolicy =
        PollyPolicies.CreatePolicy(logger);

    public async Task<Result<PaginationScheme<ProductScheme>>> GetProductsAsync(
        ProductsFetchParameters parameters, CancellationToken cancellation = default)
    {
        // applies a full resiliency pattern for external service calls using
        // timeout, retry, fallback, and circuit breaker policies.

        // more details: https://learn.microsoft.com/dotnet/architecture/resilient-applications/
        return await _productsFetchPolicy.ExecuteAsync(token =>
            productClient.GetProductsAsync(parameters, token), cancellation
        );
    }

    public async Task<Result<ProductScheme>> CreateProductAsync(
        ProductCreationScheme parameters, CancellationToken cancellation = default)
    {
        // applies a full resiliency pattern for external service calls using
        // timeout, retry, fallback, and circuit breaker policies.

        // more details: https://learn.microsoft.com/dotnet/architecture/resilient-applications/
        return await _productCreationPolicy.ExecuteAsync(token =>
            productClient.CreateProductAsync(parameters, token), cancellation
        );
    }

    public async Task<Result<ProductScheme>> UpdateProductAsync(
        ProductModificationScheme parameters, CancellationToken cancellation = default)
    {
        // applies a full resiliency pattern for external service calls using
        // timeout, retry, fallback, and circuit breaker policies.

        // more details: https://learn.microsoft.com/dotnet/architecture/resilient-applications/
        return await _productModificationPolicy.ExecuteAsync(token =>
            productClient.UpdateProductAsync(parameters, token), cancellation
        );
    }

    public async Task<Result> DeleteProductAsync(
        ProductDeletionScheme parameters, CancellationToken cancellation = default)
    {
        // applies a full resiliency pattern for external service calls using
        // timeout, retry, fallback, and circuit breaker policies.

        // more details: https://learn.microsoft.com/dotnet/architecture/resilient-applications/
        return await _productDeletionPolicy.ExecuteAsync(token =>
            productClient.DeleteProductAsync(parameters, token), cancellation
        );
    }

    public async Task<Result> UploadProductImage(
        ProductImageStreamScheme parameters, CancellationToken cancellation = default)
    {
        // applies a full resiliency pattern for external service calls using
        // timeout, retry, fallback, and circuit breaker policies.

        // more details: https://learn.microsoft.com/dotnet/architecture/resilient-applications/
        return await _productImageUploadPolicy.ExecuteAsync(token =>
            productClient.UploadProductImage(parameters, token), cancellation
        );
    }
}
