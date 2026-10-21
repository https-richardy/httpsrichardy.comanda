namespace Comanda.Internal.Contracts.Transport.Authorization;

public sealed record GroupMembershipScheme
{
    public string GroupId { get; init; }
    public string UserId { get; init; }

    public GroupMembershipScheme WithGroupId(string groupId) =>
        this with { GroupId = groupId };

    public GroupMembershipScheme WithUserId(string userId) =>
        this with { UserId = userId };
}
