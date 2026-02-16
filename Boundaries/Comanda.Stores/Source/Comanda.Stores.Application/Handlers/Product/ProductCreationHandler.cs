namespace Comanda.Stores.Application.Handlers.Product;

public sealed class ProductCreationHandler(IProductCollection collection) :
    IDispatchHandler<ProductCreationScheme, Result<ProductScheme>>
{
    public async Task<Result<ProductScheme>> HandleAsync(
        ProductCreationScheme parameters, CancellationToken cancellation = default)
    {
        var product = await collection.InsertAsync(parameters.AsProduct(), cancellation: cancellation);
        var response = product.AsResponse();

        return Result<ProductScheme>.Success(response);
    }
}
