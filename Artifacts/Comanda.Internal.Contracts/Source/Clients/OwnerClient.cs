namespace Comanda.Internal.Contracts.Clients;

public sealed class OwnerClient(HttpClient httpClient) : IOwnerClient
{
    private readonly JsonSerializerOptions serializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true
    };

    public async Task<Result<OwnerScheme>> CreateOwnerAsync(
        OwnerCreationScheme parameters, CancellationToken cancellation = default)
    {
        var response = await httpClient.PostAsJsonAsync("owners", parameters, cancellation);
        var content = await response.Content.ReadAsStringAsync(cancellation);

        var error = response.StatusCode switch
        {
            HttpStatusCode.Conflict => ProfileErrors.ProfileAlreadyExists,
            HttpStatusCode.Unauthorized => CommonErrors.UnauthorizedAccess,
            HttpStatusCode.Forbidden => CommonErrors.UnauthorizedAccess,
            HttpStatusCode.InternalServerError => CommonErrors.OperationFailed,

            _ => null
        };

        if (error is not null)
        {
            return Result<OwnerScheme>.Failure(error);
        }

        var owner = JsonSerializer.Deserialize<OwnerScheme>(content, serializerOptions);
        if (owner is null)
        {
            return Result<OwnerScheme>.Failure(CommonErrors.InvalidContent);
        }

        return Result<OwnerScheme>.Success(owner);
    }

    public async Task<Result<PaginationScheme<OwnerScheme>>> GetOwnersAsync(
        FetchOwnersParameters parameters, CancellationToken cancellation = default)
    {
        var queryString = QueryParametersParser.ToQueryString(parameters);

        var response = await httpClient.GetAsync($"owners?{queryString}", cancellation);
        var content = await response.Content.ReadAsStringAsync(cancellation);

        var error = response.StatusCode switch
        {
            HttpStatusCode.Unauthorized => CommonErrors.UnauthorizedAccess,
            HttpStatusCode.Forbidden => CommonErrors.UnauthorizedAccess,
            HttpStatusCode.InternalServerError => CommonErrors.OperationFailed,

            _ => null
        };

        if (error is not null)
        {
            return Result<PaginationScheme<OwnerScheme>>.Failure(error);
        }

        var metadataHeader = response.Headers
            .GetValues("X-Pagination")
            .FirstOrDefault();

        if (metadataHeader is null)
        {
            return Result<PaginationScheme<OwnerScheme>>.Failure(CommonErrors.InvalidContent);
        }

        var metadata = JsonSerializer.Deserialize<PaginationMetadata>(metadataHeader, serializerOptions);
        if (metadata is null)
        {
            return Result<PaginationScheme<OwnerScheme>>.Failure(CommonErrors.InvalidContent);
        }

        var items = JsonSerializer.Deserialize<IEnumerable<OwnerScheme>>(content, serializerOptions);
        if (items is null)
        {
            return Result<PaginationScheme<OwnerScheme>>.Failure(CommonErrors.InvalidContent);
        }

        var result = new PaginationScheme<OwnerScheme>
        {
            Items = [.. items],
            Total = metadata.Total,
            PageNumber = metadata.PageNumber,
            PageSize = metadata.PageSize
        };

        return Result<PaginationScheme<OwnerScheme>>.Success(result);
    }

    public async Task<Result<OwnerScheme>> UpdateOwnerAsync(
        EditOwnerScheme parameters, CancellationToken cancellation = default)
    {
        var response = await httpClient.PutAsJsonAsync($"owners/{parameters.OwnerId}", parameters, cancellation);
        var content = await response.Content.ReadAsStringAsync(cancellation);

        var error = response.StatusCode switch
        {
            HttpStatusCode.NotFound => OwnerErrors.OwnerDoesNotExist,
            HttpStatusCode.Unauthorized => CommonErrors.UnauthorizedAccess,
            HttpStatusCode.Forbidden => CommonErrors.UnauthorizedAccess,
            HttpStatusCode.InternalServerError => CommonErrors.OperationFailed,

            _ => null
        };

        if (error is not null)
        {
            return Result<OwnerScheme>.Failure(error);
        }

        var owner = JsonSerializer.Deserialize<OwnerScheme>(content, serializerOptions);
        if (owner is null)
        {
            return Result<OwnerScheme>.Failure(CommonErrors.InvalidContent);
        }

        return Result<OwnerScheme>.Success(owner);
    }

    public async Task<Result> DeleteOwnerAsync(
        OwnerDeletionScheme parameters, CancellationToken cancellation = default)
    {
        var response = await httpClient.DeleteAsync($"owners/{parameters.OwnerId}", cancellation);
        var error = response.StatusCode switch
        {
            HttpStatusCode.NotFound => OwnerErrors.OwnerDoesNotExist,
            HttpStatusCode.Unauthorized => CommonErrors.UnauthorizedAccess,
            HttpStatusCode.Forbidden => CommonErrors.UnauthorizedAccess,
            HttpStatusCode.InternalServerError => CommonErrors.OperationFailed,

            _ => null
        };

        if (error is not null)
        {
            return Result.Failure(error);
        }

        return Result.Success();
    }
}
