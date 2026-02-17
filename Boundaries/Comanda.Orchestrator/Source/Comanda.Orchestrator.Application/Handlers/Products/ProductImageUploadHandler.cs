namespace Comanda.Orchestrator.Application.Handlers.Products;

public sealed class ProductImageUploadHandler(IProductGateway productGateway) :
    IDispatchHandler<ProductImageStreamScheme, Result>
{
    public async Task<Result> HandleAsync(ProductImageStreamScheme parameters, CancellationToken cancellation = default)
    {
        return await productGateway.UploadProductImage(parameters, cancellation);
    }
}
