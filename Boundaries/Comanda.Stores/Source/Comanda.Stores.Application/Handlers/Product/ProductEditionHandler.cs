namespace Comanda.Stores.Application.Handlers.Product;

public sealed class ProductEditionHandler(IProductCollection productCollection, IEstablishmentCollection establishmentCollection) :
    IDispatchHandler<ProductEditionScheme, Result<ProductScheme>>
{
    public async Task<Result<ProductScheme>> HandleAsync(ProductEditionScheme parameters, CancellationToken cancellation = default)
    {
        var establishmentFilters = EstablishmentFilters.WithSpecifications()
            .WithIdentifier(parameters.EstablishmentId)
            .Build();

        var establishments = await establishmentCollection.FilterEstablishmentsAsync(establishmentFilters, cancellation);
        var establishment = establishments.FirstOrDefault();

        if (establishment is null)
        {
            return Result<ProductScheme>.Failure(EstablishmentErrors.EstablishmentDoesNotExist);
        }

        var productFilters = ProductFilters.WithSpecifications()
            .WithIdentifier(parameters.ProductId)
            .Build();

        var products = await productCollection.FilterProductsAsync(productFilters, cancellation);
        var product = products.FirstOrDefault();

        if (product is null)
        {
            return Result<ProductScheme>.Failure(ProductErrors.ProductDoesNotExist);
        }

        if (product.Metadata.EstablishmentId != establishment.Id)
        {
            return Result<ProductScheme>.Failure(ProductErrors.ProductBelongsToAnotherEstablishment);
        }

        var updatedProduct = await productCollection.UpdateAsync(parameters.AsProduct(product), cancellation);
        var response = updatedProduct.AsResponse();

        return Result<ProductScheme>.Success(response);
    }
}
