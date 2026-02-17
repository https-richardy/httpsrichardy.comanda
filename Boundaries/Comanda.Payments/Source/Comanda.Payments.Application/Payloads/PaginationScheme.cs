namespace Comanda.Payments.Application.Payloads;

public sealed record PaginationScheme<TItem>
{
    public IReadOnlyCollection<TItem> Items { get; init; } = [];

    public string Next { get; init; } = string.Empty;

    public bool HasMore => Next.Length > 0;

    public int Limit { get; init; }
}
