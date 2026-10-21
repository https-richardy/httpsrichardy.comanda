namespace Comanda.Internal.Contracts.Transport.Authorization;

public sealed record UserFiltersParameters
{
    public string? Id { get; init; }
    public string? Username { get; init; }
    public string? Email { get; init; }
    public bool? Enabled { get; init; }

    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 10;

    public UserFiltersParameters WithIdentifier(string? id) =>
        this with { Id = id };

    public UserFiltersParameters WithUsername(string? username) =>
        this with { Username = username };

    public UserFiltersParameters WithEmail(string? email) =>
        this with { Email = email };

    public UserFiltersParameters WithEnabled(bool? enabled) =>
        this with { Enabled = enabled };

    public UserFiltersParameters WithPage(int page) =>
        this with { Page = page };

    public UserFiltersParameters WithPageSize(int pageSize) =>
        this with { PageSize = pageSize };
}
