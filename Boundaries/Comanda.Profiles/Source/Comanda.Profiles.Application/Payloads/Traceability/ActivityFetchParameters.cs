namespace Comanda.Profiles.Application.Payloads.Traceability;

public sealed record ActivityFetchParameters :
    IDispatchable<Result<PaginationScheme<ActivityDetailsScheme>>>
{
    public string? Action { get; init; }
    public string? Resource { get; init; }
    public string? UserId { get; init; }

    public PaginationFilters Pagination { get; init; } = new();
    public SortFilters? Sort { get; init; }
}
