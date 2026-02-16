namespace Comanda.Internal.Contracts.Transport.Internal.Subscriptions;

public sealed record SubscriptionsFetchParameters :
    IDispatchable<Result<PaginationScheme<SubscriptionScheme>>>
{
    public string? Id { get; init; }
    public string? SubscriberId { get; init; }
    public string? ExternalId { get; init; }

    public Plan? Plan { get; init; }
    public PlanStatus? Status { get; init; }

    public PaginationFilters Pagination { get; init; } = PaginationFilters.From(pageNumber: 1, pageSize: 20);
    public SortFilters? Sort { get; init; }
}
