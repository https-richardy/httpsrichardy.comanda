namespace Comanda.Internal.Contracts.Transport.Internal.Stores;

public sealed record EstablishmentsFetchParameters :
    IMessage<Result<PaginationScheme<EstablishmentScheme>>>
{
    public string? Id { get; set; }
    public string? OwnerId { get; set; }
    public string? Title { get; set; }

    public PaginationFilters? Pagination { get; set; }
    public SortFilters? Sort { get; set; }

    public DateOnly? CreatedAfter { get; set; }
    public DateOnly? CreatedBefore { get; set; }
}
