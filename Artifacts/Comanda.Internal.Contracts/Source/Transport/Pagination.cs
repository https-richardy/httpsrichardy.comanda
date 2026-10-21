namespace Comanda.Internal.Contracts.Transport;

public sealed record Pagination<TItem>
{
    public int Total { get; init; }
    public int PageNumber { get; init; }
    public int PageSize { get; init; }

    public int TotalPages => (int)Math.Ceiling((double)Total / PageSize);

    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;

    public IReadOnlyCollection<TItem> Items { get; init; } = [];

    public Pagination<TItem> WithPageNumber(int pageNumber) =>
        this with { PageNumber = pageNumber };

    public Pagination<TItem> WithPageSize(int pageSize) =>
        this with { PageSize = pageSize };

    public Pagination<TItem> WithTotal(int total) =>
        this with { Total = total };

    public Pagination<TItem> WithItems(IReadOnlyCollection<TItem> items) =>
        this with { Items = items };
}
