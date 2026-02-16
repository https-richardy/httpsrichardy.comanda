namespace Comanda.Stores.Application.Handlers.Product;

public sealed class ProductDeletionHandler(IEstablishmentCollection establishmentCollection, IProductCollection productCollection) :
    IDispatchHandler<ProductDeletionScheme, Result>
{
    public async Task<Result> HandleAsync(ProductDeletionScheme parameters, CancellationToken cancellation = default)
    {
        var establishmentFilters = EstablishmentFilters.WithSpecifications()
            .WithIdentifier(parameters.EstablishmentId)
            .Build();

        var establishments = await establishmentCollection.FilterEstablishmentsAsync(establishmentFilters, cancellation);
        var existingEstablishment = establishments.FirstOrDefault();

        if (existingEstablishment is null)
        {
            /* for tracking purposes: raise error #COMANDA-ERROR-0A2DF */
            return Result.Failure(EstablishmentErrors.EstablishmentDoesNotExist);
        }

        var productFilters = ProductFilters.WithSpecifications()
            .WithIdentifier(parameters.ProductId)
            .Build();

        var products = await productCollection.FilterProductsAsync(productFilters, cancellation);
        var existingProduct = products.FirstOrDefault();

        if (existingProduct is null)
        {
            /* for tracking purposes: raise error #COMANDA-ERROR-C2C1B */
            return Result.Failure(ProductErrors.ProductDoesNotExist);
        }

        if (existingProduct.Metadata.EstablishmentId != existingEstablishment.Id)
        {
            /* for tracking purposes: raise error #COMANDA-ERROR-9FF68 */
            return Result.Failure(ProductErrors.ProductBelongsToAnotherEstablishment);
        }

        await productCollection.DeleteAsync(existingProduct, cancellation: cancellation);

        return Result.Success();
    }
}
