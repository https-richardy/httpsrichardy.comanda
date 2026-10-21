namespace Comanda.Internal.Contracts.Clients;

public sealed class RolesClient(HttpClient httpClient) : IRolesClient
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly JsonSerializerOptions _serializer = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNameCaseInsensitive = true,
    };

    public async Task<Result<Pagination<RoleScheme>>> GetRolesAsync(RoleFiltersParameters parameters, CancellationToken cancellation = default)
    {
        var offset = (parameters.Page - 1) * parameters.PageSize;
        var search = string.IsNullOrWhiteSpace(parameters.Name)
            ? string.Empty
            : $"&search={Uri.EscapeDataString(parameters.Name)}";

        var response = await _httpClient.GetAsync($"clients/{parameters.ClientIdentifier}/roles?first={offset}&max={parameters.PageSize}{search}", cancellation);
        if (!response.IsSuccessStatusCode)
        {
            return Result<Pagination<RoleScheme>>.Failure(AuthorizationErrors.ClientDoesNotExist);
        }

        var content = await response.Content.ReadAsStringAsync(cancellation);
        var roles = JsonSerializer.Deserialize<IReadOnlyCollection<RoleScheme>>(content, _serializer);

        if (roles is null)
        {
            return Result<Pagination<RoleScheme>>.Failure(Error.Unknown);
        }

        var items = roles
            .Select(role => role with { ClientIdentifier = parameters.ClientIdentifier })
            .ToList();

        var pagination = new Pagination<RoleScheme>()
            .WithItems(items)
            .WithPageNumber(parameters.Page)
            .WithPageSize(parameters.PageSize)
            .WithTotal(items.Count);

        return Result<Pagination<RoleScheme>>.Success(pagination);
    }

    public async Task<Result<RoleScheme>> GetRoleAsync(string id, CancellationToken cancellation = default)
    {
        var response = await _httpClient.GetAsync($"roles-by-id/{id}", cancellation);
        if (!response.IsSuccessStatusCode)
        {
            return Result<RoleScheme>.Failure(AuthorizationErrors.RoleDoesNotExist);
        }

        var content = await response.Content.ReadAsStringAsync(cancellation);
        var role = JsonSerializer.Deserialize<RoleRepresentation>(content, _serializer);

        if (role is null)
        {
            return Result<RoleScheme>.Failure(AuthorizationErrors.RoleDoesNotExist);
        }

        var scheme = new RoleScheme
        {
            Id = role.Id,
            Name = role.Name,
            Description = role.Description,
            ClientIdentifier = role.ContainerId
        };

        return Result<RoleScheme>.Success(scheme);
    }

    public async Task<Result<RoleScheme>> CreateRoleAsync(RoleCreationScheme role, CancellationToken cancellation = default)
    {
        var payload = new { name = role.Name, description = role.Description };
        var response = await _httpClient.PostAsJsonAsync($"clients/{role.ClientIdentifier}/roles", payload, _serializer, cancellation);

        if (!response.IsSuccessStatusCode || response.Headers.Location is null)
        {
            return Result<RoleScheme>.Failure(AuthorizationErrors.RoleAlreadyExists);
        }

        var name = Uri.UnescapeDataString(response.Headers.Location.Segments[^1]);
        var lookup = await _httpClient.GetAsync($"clients/{role.ClientIdentifier}/roles/{name}", cancellation);

        if (!lookup.IsSuccessStatusCode)
        {
            return Result<RoleScheme>.Failure(Error.Unknown);
        }

        var content = await lookup.Content.ReadAsStringAsync(cancellation);
        var created = JsonSerializer.Deserialize<RoleScheme>(content, _serializer);

        return created is null
            ? Result<RoleScheme>.Failure(Error.Unknown)
            : Result<RoleScheme>.Success(created with { ClientIdentifier = role.ClientIdentifier });
    }

    public async Task<Result<RoleScheme>> PatchRoleAsync(PatchRoleScheme role, CancellationToken cancellation = default)
    {
        var response = await _httpClient.PutAsJsonAsync($"roles-by-id/{role.Id}", role, _serializer, cancellation);

        return response.IsSuccessStatusCode
            ? await GetRoleAsync(role.Id, cancellation)
            : Result<RoleScheme>.Failure(AuthorizationErrors.RoleDoesNotExist);
    }

    public async Task<Result> DeleteRoleAsync(string id, CancellationToken cancellation = default)
    {
        var response = await _httpClient.DeleteAsync($"roles-by-id/{id}", cancellation);

        return response.IsSuccessStatusCode
            ? Result.Success()
            : Result.Failure(AuthorizationErrors.RoleDoesNotExist);
    }

    public async Task<Result> AssignUserRoleAsync(RoleAssignmentScheme assignment, CancellationToken cancellation = default)
    {
        var role = await GetRoleAsync(assignment.RoleId, cancellation);
        if (role.IsFailure || role.Data is null)
        {
            return Result.Failure(AuthorizationErrors.RoleDoesNotExist);
        }

        var payload = new[]
        {
            new { id = role.Data.Id, name = role.Data.Name }
        };

        var response = await _httpClient.PostAsJsonAsync($"users/{assignment.SubjectId}/role-mappings/clients/{role.Data.ClientIdentifier}", payload, _serializer, cancellation);

        return response.IsSuccessStatusCode
            ? Result.Success()
            : Result.Failure(AuthorizationErrors.RoleDoesNotExist);
    }

    public async Task<Result> AssignGroupRoleAsync(RoleAssignmentScheme assignment, CancellationToken cancellation = default)
    {
        var role = await GetRoleAsync(assignment.RoleId, cancellation);
        if (role.IsFailure || role.Data is null)
        {
            return Result.Failure(AuthorizationErrors.RoleDoesNotExist);
        }

        var payload = new[]
        {
            new { id = role.Data.Id, name = role.Data.Name }
        };

        var response = await _httpClient.PostAsJsonAsync($"groups/{assignment.SubjectId}/role-mappings/clients/{role.Data.ClientIdentifier}", payload, _serializer, cancellation);

        return response.IsSuccessStatusCode
            ? Result.Success()
            : Result.Failure(AuthorizationErrors.RoleDoesNotExist);
    }

    public async Task<Result> RevokeUserRoleAsync(RoleAssignmentScheme assignment, CancellationToken cancellation = default)
    {
        var role = await GetRoleAsync(assignment.RoleId, cancellation);
        if (role.IsFailure || role.Data is null)
        {
            return Result.Failure(AuthorizationErrors.RoleDoesNotExist);
        }

        var payload = new[]
        {
            new { id = role.Data.Id, name = role.Data.Name }
        };

        using var request = new HttpRequestMessage(HttpMethod.Delete, $"users/{assignment.SubjectId}/role-mappings/clients/{role.Data.ClientIdentifier}")
        {
            Content = JsonContent.Create(payload, options: _serializer)
        };

        var response = await _httpClient.SendAsync(request, cancellation);

        return response.IsSuccessStatusCode
            ? Result.Success()
            : Result.Failure(AuthorizationErrors.RoleDoesNotExist);
    }

    public async Task<Result> RevokeGroupRoleAsync(RoleAssignmentScheme assignment, CancellationToken cancellation = default)
    {
        var role = await GetRoleAsync(assignment.RoleId, cancellation);
        if (role.IsFailure || role.Data is null)
        {
            return Result.Failure(AuthorizationErrors.RoleDoesNotExist);
        }

        var payload = new[]
        {
            new { id = role.Data.Id, name = role.Data.Name }
        };

        using var request = new HttpRequestMessage(HttpMethod.Delete, $"groups/{assignment.SubjectId}/role-mappings/clients/{role.Data.ClientIdentifier}")
        {
            Content = JsonContent.Create(payload, options: _serializer)
        };

        var response = await _httpClient.SendAsync(request, cancellation);

        return response.IsSuccessStatusCode
            ? Result.Success()
            : Result.Failure(AuthorizationErrors.RoleDoesNotExist);
    }
}
