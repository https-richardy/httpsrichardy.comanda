namespace Comanda.Profiles.Application.Handlers.Owner;

public sealed class FetchOwnersHandler(IOwnerCollection collection) :
    IDispatchHandler<FetchOwnersParameters, Result<PaginationScheme<OwnerScheme>>>
{
    public async Task<Result<PaginationScheme<OwnerScheme>>> HandleAsync(FetchOwnersParameters parameters, CancellationToken cancellation = default)
    {
        var filters = OwnerFilters.WithSpecifications()
            .WithIdentifier(parameters.OwnerId)
            .WithUserId(parameters.UserId)
            .WithEmail(parameters.Email)
            .WithPhoneNumber(parameters.PhoneNumber)
            .WithIsDeleted(parameters.IsDeleted)
            .WithPagination(parameters.Pagination)
            .WithSort(parameters.Sort)
            .WithCreatedAfter(parameters.CreatedAfter)
            .WithCreatedBefore(parameters.CreatedBefore)
            .Build();

        var owners = await collection.GetOwnersAsync(filters, cancellation);
        var totalCount = await collection.CountOwnersAsync(filters, cancellation);

        var pagination = new PaginationScheme<OwnerScheme>
        {
            Items = [.. owners.Select(owner => OwnerMapper.AsResponse(owner))],
            Total = (int)totalCount,

            PageSize = parameters.Pagination?.PageSize ?? 0,
            PageNumber = parameters.Pagination?.PageNumber ?? 0
        };

        return Result<PaginationScheme<OwnerScheme>>.Success(pagination);
    }
}
