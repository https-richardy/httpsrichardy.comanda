namespace Comanda.Subscriptions.Application.Handlers.Traceability;

public sealed class FetchActivitiesHandler(IActivityCollection collection) :
    IDispatchHandler<ActivityFetchParameters, Result<PaginationScheme<ActivityDetailsScheme>>>
{
    public async Task<Result<PaginationScheme<ActivityDetailsScheme>>> HandleAsync(
        ActivityFetchParameters parameters, CancellationToken cancellation = default)
    {
        var filters = ActivityFilters.WithSpecifications()
            .WithAction(parameters.Action)
            .WithUser(parameters.UserId)
            .WithRealm(parameters.RealmId)
            .WithResource(parameters.Resource)
            .WithPagination(parameters.Pagination)
            .Build();

        var activities = await collection.GetActivitiesAsync(filters, cancellation: cancellation);
        var total = await collection.CountAsync(filters, cancellation: cancellation);

        var pagination = new PaginationScheme<ActivityDetailsScheme>
        {
            Items = [.. activities.Select(activity => ActivityMapper.AsResponse(activity))],
            Total = (int)total,
            PageNumber = parameters.Pagination.PageNumber,
            PageSize = parameters.Pagination.PageSize,
        };

        return Result<PaginationScheme<ActivityDetailsScheme>>.Success(pagination);
    }
}
