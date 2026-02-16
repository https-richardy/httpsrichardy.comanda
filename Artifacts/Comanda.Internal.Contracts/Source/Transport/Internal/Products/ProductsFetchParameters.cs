namespace Comanda.Internal.Contracts.Transport.Internal.Products;

public sealed record ProductsFetchParameters :
    IMessage<Result<PaginationScheme<ProductScheme>>>
{
    public string? Id { get; init; }
    public string? EstablishmentId { get; init; }
    public string? Title { get; init; }

    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }

    public PaginationFilters? Pagination { get; set; }
    public SortFilters? Sort { get; set; }

    public DateOnly? CreatedAfter { get; set; }
    public DateOnly? CreatedBefore { get; set; }
}
