namespace Comanda.Internal.Contracts.Clients.Interfaces;

public interface IGroupsClient
{
    public Task<Result<Pagination<GroupScheme>>> GetGroupsAsync(
        GroupFiltersParameters parameters,
        CancellationToken cancellation = default
    );

    public Task<Result<GroupScheme>> GetGroupAsync(
        string id,
        CancellationToken cancellation = default
    );

    public Task<Result<GroupScheme>> CreateGroupAsync(
        GroupCreationScheme group,
        CancellationToken cancellation = default
    );

    public Task<Result<GroupScheme>> PatchGroupAsync(
        PatchGroupScheme group,
        CancellationToken cancellation = default
    );

    public Task<Result> DeleteGroupAsync(
        string id,
        CancellationToken cancellation = default
    );

    public Task<Result<Pagination<UserScheme>>> GetGroupMembersAsync(
        GroupMembersFiltersParameters parameters,
        CancellationToken cancellation = default
    );

    public Task<Result> AssignUserGroupAsync(
        GroupMembershipScheme membership,
        CancellationToken cancellation = default
    );

    public Task<Result> RemoveUserGroupAsync(
        GroupMembershipScheme membership,
        CancellationToken cancellation = default
    );
}
