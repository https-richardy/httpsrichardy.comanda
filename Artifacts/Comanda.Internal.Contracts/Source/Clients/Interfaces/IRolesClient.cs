namespace Comanda.Internal.Contracts.Clients.Interfaces;

public interface IRolesClient
{
    public Task<Result<Pagination<RoleScheme>>> GetRolesAsync(
        RoleFiltersParameters parameters,
        CancellationToken cancellation = default
    );

    public Task<Result<RoleScheme>> GetRoleAsync(
        string id,
        CancellationToken cancellation = default
    );

    public Task<Result<RoleScheme>> CreateRoleAsync(
        RoleCreationScheme role,
        CancellationToken cancellation = default
    );

    public Task<Result<RoleScheme>> PatchRoleAsync(
        PatchRoleScheme role,
        CancellationToken cancellation = default
    );

    public Task<Result> DeleteRoleAsync(
        string id,
        CancellationToken cancellation = default
    );

    public Task<Result> AssignUserRoleAsync(
        RoleAssignmentScheme assignment,
        CancellationToken cancellation = default
    );

    public Task<Result> AssignGroupRoleAsync(
        RoleAssignmentScheme assignment,
        CancellationToken cancellation = default
    );

    public Task<Result> RevokeUserRoleAsync(
        RoleAssignmentScheme assignment,
        CancellationToken cancellation = default
    );

    public Task<Result> RevokeGroupRoleAsync(
        RoleAssignmentScheme assignment,
        CancellationToken cancellation = default
    );
}
