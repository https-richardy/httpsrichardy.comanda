namespace Comanda.Stores.Application.Handlers.Establishment;

public sealed class EstablishmentsFetchHandler(IEstablishmentCollection collection) :
    IDispatchHandler<EstablishmentsFetchParameters, Result<PaginationScheme<EstablishmentScheme>>>
{
    public async Task<Result<PaginationScheme<EstablishmentScheme>>> HandleAsync(
        EstablishmentsFetchParameters parameters, CancellationToken cancellation = default)
    {
        var filters = EstablishmentFilters.WithSpecifications()
            .WithIdentifier(parameters.Id)
            .WithTitle(parameters.Title)
            .WithOwnerId(parameters.OwnerId)
            .WithPagination(parameters.Pagination)
            .WithSort(parameters.Sort)
            .WithCreatedAfter(parameters.CreatedAfter)
            .WithCreatedBefore(parameters.CreatedBefore)
            .Build();

        var establishments = await collection.FilterEstablishmentsAsync(filters, cancellation);
        var totalCount = await collection.CountEstablishmentsAsync(filters, cancellation);

        var pagination = new PaginationScheme<EstablishmentScheme>
        {
            Items = [.. establishments.Select(establishment => establishment.AsResponse())],
            Total = (int)totalCount,
            PageNumber = parameters.Pagination?.PageNumber ?? 1,
            PageSize = parameters.Pagination?.PageSize ?? 20,
        };

        return Result<PaginationScheme<EstablishmentScheme>>.Success(pagination);
    }
}
