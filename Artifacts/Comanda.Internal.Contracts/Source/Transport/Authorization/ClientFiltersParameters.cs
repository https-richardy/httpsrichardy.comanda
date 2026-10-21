namespace Comanda.Internal.Contracts.Transport.Authorization;

public sealed record ClientFiltersParameters
{
    public string? Id { get; init; }
    public string? ClientId { get; init; }
    public bool? Enabled { get; init; }

    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 10;

    public ClientFiltersParameters WithIdentifier(string? id) =>
        this with { Id = id };

    public ClientFiltersParameters WithClientId(string? clientId) =>
        this with { ClientId = clientId };

    public ClientFiltersParameters WithEnabled(bool? enabled) =>
        this with { Enabled = enabled };

    public ClientFiltersParameters WithPage(int page) =>
        this with { Page = page };

    public ClientFiltersParameters WithPageSize(int pageSize) =>
        this with { PageSize = pageSize };
}
