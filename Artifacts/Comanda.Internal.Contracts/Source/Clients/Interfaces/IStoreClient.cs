namespace Comanda.Internal.Contracts.Clients.Interfaces;

public interface IStoreClient
{
    public Task<Result<PaginationScheme<EstablishmentScheme>>> GetEstablishmentsAsync(
        EstablishmentsFetchParameters parameters,
        CancellationToken cancellation = default
    );

    public Task<Result<EstablishmentScheme>> CreateEstablishmentAsync(
        EstablishmentCreationScheme parameters,
        CancellationToken cancellation = default
    );

    public Task<Result<EstablishmentScheme>> UpdateEstablishmentAsync(
        EstablishmentModificationScheme parameters,
        CancellationToken cancellation = default
    );

    public Task<Result> DeleteEstablishmentAsync(
        EstablishmentDeletionScheme parameters,
        CancellationToken cancellation = default
    );

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

    public Task<Result<IReadOnlyCollection<CredentialScheme>>> GetCredentialsAsync(
        CredentialsFetchParameters parameters,
        CancellationToken cancellation = default
    );

    public Task<Result<CredentialScheme>> AssignIntegrationCredentialAsync(
        CredentialCreationScheme parameters,
        CancellationToken cancellation = default
    );

    public Task<Result<CredentialScheme>> UpdateCredentialAsync(
        CredentialModificationScheme parameters,
        CancellationToken cancellation = default
    );
}
