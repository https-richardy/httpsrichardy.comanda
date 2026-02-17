namespace Comanda.Orchestrator.Application.Handlers.Products;

public sealed class ProductModificationHandler(IProductGateway productGateway) :
    IDispatchHandler<ProductModificationScheme, Result<ProductScheme>>
{
    public async Task<Result<ProductScheme>> HandleAsync(
        ProductModificationScheme parameters, CancellationToken cancellation = default)
    {
        return await productGateway.UpdateProductAsync(parameters, cancellation);
    }
}
