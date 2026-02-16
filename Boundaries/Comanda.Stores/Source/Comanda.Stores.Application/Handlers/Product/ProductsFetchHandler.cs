namespace Comanda.Stores.Application.Handlers.Product;

public sealed class ProductsFetchHandler(IProductCollection collection) :
    IDispatchHandler<ProductsFetchParameters, Result<PaginationScheme<ProductScheme>>>
{
    public async Task<Result<PaginationScheme<ProductScheme>>> HandleAsync(
        ProductsFetchParameters parameters, CancellationToken cancellation = default)
    {
        var filters = ProductFilters.WithSpecifications()
            .WithIdentifier(parameters.Id)
            .WithEstablishmentId(parameters.EstablishmentId)
            .WithTitle(parameters.Title)
            .WithMaxPrice(parameters.MaxPrice)
            .WithMinPrice(parameters.MinPrice)
            .WithPagination(parameters.Pagination)
            .WithSort(parameters.Sort)
            .WithCreatedAfter(parameters.CreatedAfter)
            .WithCreatedBefore(parameters.CreatedBefore)
            .Build();

        var products = await collection.FilterProductsAsync(filters, cancellation);
        var totalCount = await collection.CountProductsAsync(filters, cancellation);

        var pagination = new PaginationScheme<ProductScheme>
        {
            Items = [.. products.Select(product => product.AsResponse())],
            Total = (int)totalCount,
            PageNumber = parameters.Pagination?.PageNumber ?? 1,
            PageSize = parameters.Pagination?.PageSize ?? 20,
        };

        return Result<PaginationScheme<ProductScheme>>.Success(pagination);
    }
}
