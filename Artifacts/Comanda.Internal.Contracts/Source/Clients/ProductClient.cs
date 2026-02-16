namespace Comanda.Internal.Contracts.Clients;

public sealed class ProductClient(HttpClient httpClient) : IProductClient
{
    private readonly JsonSerializerOptions serializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true
    };

    public async Task<Result<PaginationScheme<ProductScheme>>> GetProductsAsync(
        ProductsFetchParameters parameters, CancellationToken cancellation = default)
    {
        var queryString = QueryParametersParser.ToQueryString(parameters);

        var response = await httpClient.GetAsync($"establishments/{parameters.EstablishmentId}/products?{queryString}", cancellation);
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
        var response = await httpClient.PostAsJsonAsync($"establishments/{parameters.EstablishmentId}/products", parameters, cancellation);
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
        var response = await httpClient.PutAsJsonAsync($"establishments/{parameters.EstablishmentId}/products/{parameters.ProductId}", parameters, cancellation);
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
        var response = await httpClient.DeleteAsync($"establishments/{parameters.EstablishmentId}/products/{parameters.ProductId}", cancellation);
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

        var response = await httpClient.PutAsync($"establishments/{parameters.EstablishmentId}/products/{parameters.ProductId}/image", form, cancellation);
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
}
