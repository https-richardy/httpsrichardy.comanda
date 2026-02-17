namespace Comanda.Orchestrator.Application.Handlers.Products;

public sealed class ProductCreationHandler(IProductGateway productGateway) :
    IDispatchHandler<ProductCreationScheme, Result<ProductScheme>>
{
    public async Task<Result<ProductScheme>> HandleAsync(
        ProductCreationScheme parameters, CancellationToken cancellation = default)
    {
        return await productGateway.CreateProductAsync(parameters, cancellation);
    }
}
