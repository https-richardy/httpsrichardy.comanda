namespace Comanda.Internal.Contracts.Clients;

public sealed class EstablishmentClient(HttpClient httpClient) : IEstablishmentClient
{
    private readonly JsonSerializerOptions serializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true
    };

    public async Task<Result<PaginationScheme<EstablishmentScheme>>> GetEstablishmentsAsync(
        EstablishmentsFetchParameters parameters, CancellationToken cancellation = default)
    {
        var queryString = QueryParametersParser.ToQueryString(parameters);

        var response = await httpClient.GetAsync($"establishments?{queryString}", cancellation);
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
            return Result<PaginationScheme<EstablishmentScheme>>.Failure(error);
        }

        var metadataHeader = response.Headers
            .GetValues("X-Pagination")
            .FirstOrDefault();

        if (metadataHeader is null)
        {
            return Result<PaginationScheme<EstablishmentScheme>>.Failure(CommonErrors.InvalidContent);
        }

        var metadata = JsonSerializer.Deserialize<PaginationMetadata>(metadataHeader, serializerOptions);
        if (metadata is null)
        {
            return Result<PaginationScheme<EstablishmentScheme>>.Failure(CommonErrors.InvalidContent);
        }

        var items = JsonSerializer.Deserialize<IEnumerable<EstablishmentScheme>>(content, serializerOptions);
        if (items is null)
        {
            return Result<PaginationScheme<EstablishmentScheme>>.Failure(CommonErrors.InvalidContent);
        }

        var result = new PaginationScheme<EstablishmentScheme>
        {
            Items = [.. items],
            Total = metadata.Total,
            PageNumber = metadata.PageNumber,
            PageSize = metadata.PageSize
        };

        return Result<PaginationScheme<EstablishmentScheme>>.Success(result);
    }

    public async Task<Result<EstablishmentScheme>> CreateEstablishmentAsync(
        EstablishmentCreationScheme parameters, CancellationToken cancellation = default)
    {
        var response = await httpClient.PostAsJsonAsync("establishments", parameters, cancellation);
        var content = await response.Content.ReadAsStringAsync(cancellation);

        var error = response.StatusCode switch
        {
            HttpStatusCode.Conflict => EstablishmentErrors.OwnerAlreadyHasEstablishment,
            HttpStatusCode.Unauthorized => CommonErrors.UnauthorizedAccess,
            HttpStatusCode.Forbidden => CommonErrors.UnauthorizedAccess,
            HttpStatusCode.InternalServerError => CommonErrors.OperationFailed,

            _ => null
        };

        if (error is not null)
        {
            return Result<EstablishmentScheme>.Failure(error);
        }

        var establishment = JsonSerializer.Deserialize<EstablishmentScheme>(content, serializerOptions);
        if (establishment is null)
        {
            return Result<EstablishmentScheme>.Failure(CommonErrors.InvalidContent);
        }

        return Result<EstablishmentScheme>.Success(establishment);
    }

    public async Task<Result<EstablishmentScheme>> UpdateEstablishmentAsync(
        EstablishmentModificationScheme parameters, CancellationToken cancellation = default)
    {
        var response = await httpClient.PutAsJsonAsync($"establishments/{parameters.EstablishmentId}", parameters, cancellation);
        var content = await response.Content.ReadAsStringAsync(cancellation);

        var error = response.StatusCode switch
        {
            HttpStatusCode.NotFound => EstablishmentErrors.EstablishmentDoesNotExist,
            HttpStatusCode.Unauthorized => CommonErrors.UnauthorizedAccess,
            HttpStatusCode.Forbidden => CommonErrors.UnauthorizedAccess,
            HttpStatusCode.InternalServerError => CommonErrors.OperationFailed,

            _ => null
        };

        if (error is not null)
        {
            return Result<EstablishmentScheme>.Failure(error);
        }

        var establishment = JsonSerializer.Deserialize<EstablishmentScheme>(content, serializerOptions);
        if (establishment is null)
        {
            return Result<EstablishmentScheme>.Failure(CommonErrors.InvalidContent);
        }

        return Result<EstablishmentScheme>.Success(establishment);
    }

    public async Task<Result> DeleteEstablishmentAsync(
        EstablishmentDeletionScheme parameters, CancellationToken cancellation = default)
    {
        var response = await httpClient.DeleteAsync($"establishments/{parameters.EstablishmentId}", cancellation);
        var error = response.StatusCode switch
        {
            HttpStatusCode.NotFound => EstablishmentErrors.EstablishmentDoesNotExist,
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
