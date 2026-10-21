namespace Comanda.Internal.Contracts.Transport.Authorization;

public sealed record RoleFiltersParameters
{
    public string ClientIdentifier { get; init; }

    public string? Id { get; init; }
    public string? Name { get; init; }

    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 10;

    public RoleFiltersParameters WithClientIdentifier(string clientIdentifier) =>
        this with { ClientIdentifier = clientIdentifier };

    public RoleFiltersParameters WithIdentifier(string? id) =>
        this with { Id = id };

    public RoleFiltersParameters WithName(string? name) =>
        this with { Name = name };

    public RoleFiltersParameters WithPage(int page) =>
        this with { Page = page };

    public RoleFiltersParameters WithPageSize(int pageSize) =>
        this with { PageSize = pageSize };
}
