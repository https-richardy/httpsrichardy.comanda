namespace Comanda.Orchestrator.Application.Handlers.Products;

public sealed class ProductsFetchHandler(IProductGateway productGateway) :
    IDispatchHandler<ProductsFetchParameters, Result<PaginationScheme<ProductScheme>>>
{
    public async Task<Result<PaginationScheme<ProductScheme>>> HandleAsync(
        ProductsFetchParameters parameters, CancellationToken cancellation = default)
    {
        return await productGateway.GetProductsAsync(parameters, cancellation);
    }
}
