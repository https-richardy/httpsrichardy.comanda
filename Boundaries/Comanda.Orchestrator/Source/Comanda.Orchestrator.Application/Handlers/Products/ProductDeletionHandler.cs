namespace Comanda.Orchestrator.Application.Handlers.Products;

public sealed class ProductDeletionHandler(IProductGateway productGateway) :
    IDispatchHandler<ProductDeletionScheme, Result>
{
    public async Task<Result> HandleAsync(
        ProductDeletionScheme parameters, CancellationToken cancellation = default)
    {
        return await productGateway.DeleteProductAsync(parameters, cancellation);
    }
}
