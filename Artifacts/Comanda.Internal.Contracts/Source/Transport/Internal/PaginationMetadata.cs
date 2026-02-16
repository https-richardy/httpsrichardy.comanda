namespace Comanda.Internal.Contracts.Transport.Internal;

public sealed record PaginationMetadata
{
    public int Total { get; init; }
    public int TotalPages { get; init; }

    public int PageNumber { get; init; }
    public int PageSize { get; init; }

    public bool HasNext { get; init; }
    public bool HasPrevious { get; init; }
}
