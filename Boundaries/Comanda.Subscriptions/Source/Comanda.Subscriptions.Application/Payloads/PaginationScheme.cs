namespace Comanda.Subscriptions.Application.Payloads;

public sealed record PaginationScheme<TItem>
{
    public IReadOnlyCollection<TItem> Items { get; init; } = [];

    public int Total { get; init; }
    public int PageNumber { get; init; }
    public int PageSize { get; init; }

    public int TotalPages =>
        PageSize > 0 ? (int)Math.Ceiling((double)Total / PageSize) : 1;

    public bool HasPreviousPage =>
        PageNumber > 0 && TotalPages > 1;

    public bool HasNextPage =>
        PageNumber + 1 < TotalPages;
}