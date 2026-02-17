namespace Comanda.Orchestrator.Application.Gateways;

public interface IProductGateway
{
    public Task<Result<PaginationScheme<ProductScheme>>> GetProductsAsync(
        ProductsFetchParameters parameters,
        CancellationToken cancellation = default
    );

    public Task<Result<ProductScheme>> CreateProductAsync(
        ProductCreationScheme parameters,
        CancellationToken cancellation = default
    );

    public Task<Result<ProductScheme>> UpdateProductAsync(
        ProductModificationScheme parameters,
        CancellationToken cancellation = default
    );

    public Task<Result> DeleteProductAsync(
        ProductDeletionScheme parameters,
        CancellationToken cancellation = default
    );

    public Task<Result> UploadProductImage(
        ProductImageStreamScheme parameters,
        CancellationToken cancellation = default
    );
}
