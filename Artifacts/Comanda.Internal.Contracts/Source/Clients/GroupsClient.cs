namespace Comanda.Internal.Contracts.Clients;

public sealed class GroupsClient(HttpClient httpClient) : IGroupsClient
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly JsonSerializerOptions _serializer = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNameCaseInsensitive = true,
    };

    public async Task<Result<Pagination<GroupScheme>>> GetGroupsAsync(GroupFiltersParameters parameters, CancellationToken cancellation = default)
    {
        var offset = (parameters.Page - 1) * parameters.PageSize;
        var search = string.IsNullOrWhiteSpace(parameters.Name) ? string.Empty : $"&search={Uri.EscapeDataString(parameters.Name)}";

        var response = await _httpClient.GetAsync($"groups?first={offset}&max={parameters.PageSize}{search}", cancellation);
        var content = await response.Content.ReadAsStringAsync(cancellation);

        if (!response.IsSuccessStatusCode)
        {
            return Result<Pagination<GroupScheme>>.Failure(Error.Unknown);
        }

        var groups = JsonSerializer.Deserialize<IReadOnlyCollection<GroupScheme>>(content, _serializer);
        if (groups is null)
        {
            return Result<Pagination<GroupScheme>>.Failure(Error.Unknown);
        }

        var pagination = new Pagination<GroupScheme>()
            .WithItems(groups)
            .WithPageNumber(parameters.Page)
            .WithPageSize(parameters.PageSize)
            .WithTotal(groups.Count);

        return Result<Pagination<GroupScheme>>.Success(pagination);
    }

    public async Task<Result<GroupScheme>> GetGroupAsync(string id, CancellationToken cancellation = default)
    {
        var response = await _httpClient.GetAsync($"groups/{id}", cancellation);
        var content = await response.Content.ReadAsStringAsync(cancellation);

        if (!response.IsSuccessStatusCode)
        {
            return Result<GroupScheme>.Failure(AuthorizationErrors.GroupDoesNotExist);
        }

        var group = JsonSerializer.Deserialize<GroupScheme>(content, _serializer);

        return group is null
            ? Result<GroupScheme>.Failure(Error.Unknown)
            : Result<GroupScheme>.Success(group);
    }

    public async Task<Result<GroupScheme>> CreateGroupAsync(GroupCreationScheme group, CancellationToken cancellation = default)
    {
        var response = await _httpClient.PostAsJsonAsync("groups", group, _serializer, cancellation);
        if (!response.IsSuccessStatusCode || response.Headers.Location is null)
        {
            return Result<GroupScheme>.Failure(AuthorizationErrors.GroupAlreadyExists);
        }

        return await GetGroupAsync(response.Headers.Location.Segments[^1], cancellation);
    }

    public async Task<Result<GroupScheme>> PatchGroupAsync(PatchGroupScheme group, CancellationToken cancellation = default)
    {
        var response = await _httpClient.PutAsJsonAsync($"groups/{group.Id}", group, _serializer, cancellation);

        return response.IsSuccessStatusCode
            ? await GetGroupAsync(group.Id, cancellation)
            : Result<GroupScheme>.Failure(AuthorizationErrors.GroupDoesNotExist);
    }

    public async Task<Result> DeleteGroupAsync(string id, CancellationToken cancellation = default)
    {
        var response = await _httpClient.DeleteAsync($"groups/{id}", cancellation);

        return response.IsSuccessStatusCode
            ? Result.Success()
            : Result.Failure(AuthorizationErrors.GroupDoesNotExist);
    }

    public async Task<Result<Pagination<UserScheme>>> GetGroupMembersAsync(GroupMembersFiltersParameters parameters, CancellationToken cancellation = default)
    {
        var offset = (parameters.Page - 1) * parameters.PageSize;

        var response = await _httpClient.GetAsync($"groups/{parameters.GroupId}/members?first={offset}&max={parameters.PageSize}", cancellation);
        var content = await response.Content.ReadAsStringAsync(cancellation);

        if (!response.IsSuccessStatusCode)
        {
            return Result<Pagination<UserScheme>>.Failure(AuthorizationErrors.GroupDoesNotExist);
        }

        var members = JsonSerializer.Deserialize<IReadOnlyCollection<UserScheme>>(content, _serializer);
        if (members is null)
        {
            return Result<Pagination<UserScheme>>.Failure(Error.Unknown);
        }

        var pagination = new Pagination<UserScheme>()
            .WithItems(members)
            .WithPageNumber(parameters.Page)
            .WithPageSize(parameters.PageSize)
            .WithTotal(members.Count);

        return Result<Pagination<UserScheme>>.Success(pagination);
    }

    public async Task<Result> AssignUserGroupAsync(GroupMembershipScheme membership, CancellationToken cancellation = default)
    {
        var content = new StringContent(string.Empty);
        var response = await _httpClient.PutAsync($"users/{membership.UserId}/groups/{membership.GroupId}", content, cancellation);

        return response.IsSuccessStatusCode
            ? Result.Success()
            : Result.Failure(AuthorizationErrors.GroupDoesNotExist);
    }

    public async Task<Result> RemoveUserGroupAsync(GroupMembershipScheme membership, CancellationToken cancellation = default)
    {
        var response = await _httpClient.DeleteAsync($"users/{membership.UserId}/groups/{membership.GroupId}", cancellation);

        return response.IsSuccessStatusCode
            ? Result.Success()
            : Result.Failure(AuthorizationErrors.GroupDoesNotExist);
    }
}
