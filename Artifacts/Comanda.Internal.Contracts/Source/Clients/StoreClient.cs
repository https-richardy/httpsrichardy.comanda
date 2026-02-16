namespace Comanda.Internal.Contracts.Clients;

public sealed class StoreClient(HttpClient httpClient) : IStoreClient
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

        var response = await httpClient.GetAsync($"stores?{queryString}", cancellation);
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
        var response = await httpClient.PostAsJsonAsync("stores", parameters, cancellation);
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
        var response = await httpClient.PutAsJsonAsync($"stores/{parameters.EstablishmentId}", parameters, cancellation);
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
        var response = await httpClient.DeleteAsync($"stores/{parameters.EstablishmentId}", cancellation);
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

    public async Task<Result<PaginationScheme<ProductScheme>>> GetProductsAsync(
        ProductsFetchParameters parameters, CancellationToken cancellation = default)
    {
        var queryString = QueryParametersParser.ToQueryString(parameters);

        var response = await httpClient.GetAsync($"stores/{parameters.EstablishmentId}/products?{queryString}", cancellation);
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
            return Result<PaginationScheme<ProductScheme>>.Failure(error);
        }

        var metadataHeader = response.Headers
            .GetValues("X-Pagination")
            .FirstOrDefault();

        if (metadataHeader is null)
        {
            return Result<PaginationScheme<ProductScheme>>.Failure(CommonErrors.InvalidContent);
        }

        var metadata = JsonSerializer.Deserialize<PaginationMetadata>(metadataHeader, serializerOptions);
        if (metadata is null)
        {
            return Result<PaginationScheme<ProductScheme>>.Failure(CommonErrors.InvalidContent);
        }

        var items = JsonSerializer.Deserialize<IEnumerable<ProductScheme>>(content, serializerOptions);
        if (items is null)
        {
            return Result<PaginationScheme<ProductScheme>>.Failure(CommonErrors.InvalidContent);
        }

        var result = new PaginationScheme<ProductScheme>
        {
            Items = [.. items],
            Total = metadata.Total,
            PageNumber = metadata.PageNumber,
            PageSize = metadata.PageSize
        };

        return Result<PaginationScheme<ProductScheme>>.Success(result);
    }

    public async Task<Result<ProductScheme>> CreateProductAsync(
        ProductCreationScheme parameters, CancellationToken cancellation = default)
    {
        var response = await httpClient.PostAsJsonAsync($"stores/{parameters.EstablishmentId}/products", parameters, cancellation);
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
            return Result<ProductScheme>.Failure(error);
        }

        var product = JsonSerializer.Deserialize<ProductScheme>(content, serializerOptions);
        if (product is null)
        {
            return Result<ProductScheme>.Failure(CommonErrors.InvalidContent);
        }

        return Result<ProductScheme>.Success(product);
    }

    public async Task<Result<ProductScheme>> UpdateProductAsync(
        ProductModificationScheme parameters, CancellationToken cancellation = default)
    {
        var response = await httpClient.PutAsJsonAsync($"stores/{parameters.EstablishmentId}/products/{parameters.ProductId}", parameters, cancellation);
        var content = await response.Content.ReadAsStringAsync(cancellation);

        if (!response.IsSuccessStatusCode)
        {
            var error = JsonSerializer.Deserialize<Error>(content, serializerOptions);
            if (error is not null)
            {
                return Result<ProductScheme>.Failure(error);
            }

            return Result<ProductScheme>.Failure(CommonErrors.OperationFailed);
        }

        var product = JsonSerializer.Deserialize<ProductScheme>(content, serializerOptions);
        if (product is null)
        {
            return Result<ProductScheme>.Failure(CommonErrors.InvalidContent);
        }

        return Result<ProductScheme>.Success(product);
    }

    public async Task<Result> DeleteProductAsync(
        ProductDeletionScheme parameters, CancellationToken cancellation = default)
    {
        var response = await httpClient.DeleteAsync($"stores/{parameters.EstablishmentId}/products/{parameters.ProductId}", cancellation);
        var content = await response.Content.ReadAsStringAsync(cancellation);

        if (!response.IsSuccessStatusCode)
        {
            var error = JsonSerializer.Deserialize<Error>(content, serializerOptions);
            if (error is not null)
            {
                return Result.Failure(error);
            }

            return Result.Failure(CommonErrors.OperationFailed);
        }

        return Result.Success();
    }

    public async Task<Result> UploadProductImage(
        ProductImageStreamScheme parameters, CancellationToken cancellation = default)
    {
        using var form = new MultipartFormDataContent();
        using var stream = new StreamContent(parameters.Stream);

        stream.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");
        form.Add(stream, "file", "product.jpg");

        var response = await httpClient.PutAsync($"stores/{parameters.EstablishmentId}/products/{parameters.ProductId}/image", form, cancellation);
        var content = await response.Content.ReadAsStringAsync(cancellation);

        if (!response.IsSuccessStatusCode)
        {
            var error = JsonSerializer.Deserialize<Error>(content, serializerOptions);
            if (error is not null)
            {
                return Result.Failure(error);
            }

            return Result.Failure(CommonErrors.OperationFailed);
        }

        return Result.Success();
    }

    public async Task<Result<IReadOnlyCollection<CredentialScheme>>> GetCredentialsAsync(
        CredentialsFetchParameters parameters, CancellationToken cancellation = default)
    {
        var response = await httpClient.GetAsync($"stores/{parameters.EstablishmentId}/integrations/credentials", cancellation);
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
            return Result<IReadOnlyCollection<CredentialScheme>>.Failure(error);
        }

        var credentials = JsonSerializer.Deserialize<IReadOnlyCollection<CredentialScheme>>(content, serializerOptions);
        if (credentials is null)
        {
            return Result<IReadOnlyCollection<CredentialScheme>>.Failure(CommonErrors.InvalidContent);
        }

        return Result<IReadOnlyCollection<CredentialScheme>>.Success(credentials);
    }

    public async Task<Result<CredentialScheme>> AssignIntegrationCredentialAsync(
        CredentialCreationScheme parameters, CancellationToken cancellation = default)
    {
        var response = await httpClient.PostAsJsonAsync($"stores/{parameters.EstablishmentId}/integrations/credentials", parameters, cancellation);
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
            return Result<CredentialScheme>.Failure(error);
        }

        var credential = JsonSerializer.Deserialize<CredentialScheme>(content, serializerOptions);
        if (credential is null)
        {
            return Result<CredentialScheme>.Failure(CommonErrors.InvalidContent);
        }

        return Result<CredentialScheme>.Success(credential);
    }

    public async Task<Result<CredentialScheme>> UpdateCredentialAsync(
        CredentialModificationScheme parameters, CancellationToken cancellation = default)
    {
        var response = await httpClient.PutAsJsonAsync($"stores/{parameters.EstablishmentId}/integrations/credentials/{parameters.CredentialId}", parameters, cancellation);
        var content = await response.Content.ReadAsStringAsync(cancellation);

        if (!response.IsSuccessStatusCode)
        {
            var error = JsonSerializer.Deserialize<Error>(content, serializerOptions);
            if (error is not null)
                return Result<CredentialScheme>.Failure(error);

            return Result<CredentialScheme>.Failure(CommonErrors.OperationFailed);
        }

        var credential = JsonSerializer.Deserialize<CredentialScheme>(content, serializerOptions);
        if (credential is null)
        {
            return Result<CredentialScheme>.Failure(CommonErrors.InvalidContent);
        }

        return Result<CredentialScheme>.Success(credential);
    }
}
