namespace Comanda.Internal.Contracts.Transport.Authorization;

public sealed record GroupFiltersParameters
{
    public string? Id { get; init; }
    public string? Name { get; init; }

    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 10;

    public GroupFiltersParameters WithIdentifier(string? id) =>
        this with { Id = id };

    public GroupFiltersParameters WithName(string? name) =>
        this with { Name = name };

    public GroupFiltersParameters WithPage(int page) =>
        this with { Page = page };

    public GroupFiltersParameters WithPageSize(int pageSize) =>
        this with { PageSize = pageSize };
}
