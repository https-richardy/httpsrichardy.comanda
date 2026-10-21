namespace Comanda.Internal.Contracts.Transport.Authorization;

public sealed record GroupMembersFiltersParameters
{
    public string GroupId { get; init; }

    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 10;

    public GroupMembersFiltersParameters WithGroupId(string groupId) =>
        this with { GroupId = groupId };

    public GroupMembersFiltersParameters WithPage(int page) =>
        this with { Page = page };

    public GroupMembersFiltersParameters WithPageSize(int pageSize) =>
        this with { PageSize = pageSize };
}
