namespace Comanda.Stores.Application.Handlers.Product;

public sealed class ProductImageUploadHandler(IProductCollection productCollection, IEstablishmentCollection establishmentCollection, IFileGateway fileGateway) :
    IDispatchHandler<ProductImageUploadScheme, Result>
{
    public async Task<Result> HandleAsync(ProductImageUploadScheme parameters, CancellationToken cancellation = default)
    {
        var productFilters = ProductFilters.WithSpecifications()
            .WithIdentifier(parameters.ProductId)
            .WithEstablishmentId(parameters.EstablishmentId)
            .Build();

        var establishmentsFilters = EstablishmentFilters.WithSpecifications()
            .WithIdentifier(parameters.EstablishmentId)
            .Build();

        var establishments = await establishmentCollection.FilterEstablishmentsAsync(establishmentsFilters, cancellation);
        var establishment = establishments.FirstOrDefault();

        if (establishment is null)
        {
            return Result.Failure(EstablishmentErrors.EstablishmentDoesNotExist);
        }

        var products = await productCollection.FilterProductsAsync(productFilters, cancellation);
        var product = products.FirstOrDefault();

        if (product is null)
        {
            return Result.Failure(ProductErrors.ProductDoesNotExist);
        }

        product.Image = await fileGateway.UploadFileAsync(parameters.Stream, cancellation);

        await productCollection.UpdateAsync(product, cancellation);

        return Result.Success();
    }
}
