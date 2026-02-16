namespace Comanda.Stores.Application.Payloads.Establishment;

public sealed record EstablishmentsFetchParameters :
    IDispatchable<Result<PaginationScheme<EstablishmentScheme>>>
{
    public string? Id { get; set; }
    public string? OwnerId { get; set; }
    public string? Title { get; set; }

    public PaginationFilters? Pagination { get; set; }
    public SortFilters? Sort { get; set; }

    public DateOnly? CreatedAfter { get; set; }
    public DateOnly? CreatedBefore { get; set; }
}
