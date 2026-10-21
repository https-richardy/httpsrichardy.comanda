namespace Comanda.Internal.Contracts.Clients.Interfaces;

public interface IUsersClient
{
    public Task<Result<Pagination<UserScheme>>> GetUsersAsync(
        UserFiltersParameters parameters,
        CancellationToken cancellation = default
    );

    public Task<Result<UserScheme>> GetUserAsync(
        string id,
        CancellationToken cancellation = default
    );

    public Task<Result<UserScheme>> CreateUserAsync(
        UserCreationScheme user,
        CancellationToken cancellation = default
    );

    public Task<Result<UserScheme>> PatchUserAsync(
        PatchUserScheme user,
        CancellationToken cancellation = default
    );

    public Task<Result> DeleteUserAsync(
        string id,
        CancellationToken cancellation = default
    );
}
