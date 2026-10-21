namespace Comanda.Internal.Contracts.Clients;

public sealed class UsersClient(HttpClient httpClient) : IUsersClient
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly JsonSerializerOptions _serializer = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNameCaseInsensitive = true,
    };

    public async Task<Result<Pagination<UserScheme>>> GetUsersAsync(UserFiltersParameters parameters, CancellationToken cancellation = default)
    {
        var queryString = QueryParametersParser.ToQueryString(parameters);
        var offset = (parameters.Page - 1) * parameters.PageSize;

        var response = await _httpClient.GetAsync($"users?{queryString}&first={offset}&max={parameters.PageSize}", cancellation);
        var content = await response.Content.ReadAsStringAsync(cancellation);

        if (!response.IsSuccessStatusCode)
        {
            return Result<Pagination<UserScheme>>.Failure(Error.Unknown);
        }

        var users = JsonSerializer.Deserialize<IReadOnlyCollection<UserScheme>>(content, _serializer);
        if (users is null)
        {
            return Result<Pagination<UserScheme>>.Failure(Error.Unknown);
        }

        var pagination = new Pagination<UserScheme>()
            .WithItems(users)
            .WithPageNumber(parameters.Page)
            .WithPageSize(parameters.PageSize)
            .WithTotal(users.Count);

        return Result<Pagination<UserScheme>>.Success(pagination);
    }

    public async Task<Result<UserScheme>> GetUserAsync(string id, CancellationToken cancellation = default)
    {
        var response = await _httpClient.GetAsync($"users/{id}", cancellation);
        var content = await response.Content.ReadAsStringAsync(cancellation);

        if (!response.IsSuccessStatusCode)
        {
            return Result<UserScheme>.Failure(AuthorizationErrors.UserDoesNotExist);
        }

        var user = JsonSerializer.Deserialize<UserScheme>(content, _serializer);

        return user is null
            ? Result<UserScheme>.Failure(AuthorizationErrors.UserDoesNotExist)
            : Result<UserScheme>.Success(user);
    }

    public async Task<Result<UserScheme>> CreateUserAsync(UserCreationScheme user, CancellationToken cancellation = default)
    {
        var response = await _httpClient.PostAsJsonAsync("users", user, _serializer, cancellation);
        if (!response.IsSuccessStatusCode || response.Headers.Location is null)
        {
            return Result<UserScheme>.Failure(AuthorizationErrors.UserAlreadyExists);
        }

        return await GetUserAsync(response.Headers.Location.Segments[^1], cancellation);
    }

    public async Task<Result<UserScheme>> PatchUserAsync(PatchUserScheme user, CancellationToken cancellation = default)
    {
        var response = await _httpClient.PutAsJsonAsync($"users/{user.Id}", user, _serializer, cancellation);

        return response.IsSuccessStatusCode
            ? await GetUserAsync(user.Id, cancellation)
            : Result<UserScheme>.Failure(AuthorizationErrors.UserDoesNotExist);
    }

    public async Task<Result> DeleteUserAsync(string id, CancellationToken cancellation = default)
    {
        var response = await _httpClient.DeleteAsync($"users/{id}", cancellation);

        return response.IsSuccessStatusCode
            ? Result.Success()
            : Result.Failure(AuthorizationErrors.UserDoesNotExist);
    }
}
