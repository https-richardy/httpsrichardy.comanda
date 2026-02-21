namespace Comanda.Internal.Contracts.Clients;

public sealed class OrderClient(HttpClient httpClient) : IOrderClient
{
    private readonly JsonSerializerOptions serializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true
    };

    public async Task<Result<PaginationScheme<OrderScheme>>> GetOrdersAsync(OrdersFetchParameters parameters, CancellationToken cancellation = default)
    {
        var queryString = QueryParametersParser.ToQueryString(parameters);

        var response = await httpClient.GetAsync($"orders?{queryString}", cancellation);
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
            return Result<PaginationScheme<OrderScheme>>.Failure(error);
        }

        var metadataHeader = response.Headers
            .GetValues("X-Pagination")
            .FirstOrDefault();

        if (metadataHeader is null)
        {
            return Result<PaginationScheme<OrderScheme>>.Failure(CommonErrors.InvalidContent);
        }

        var metadata = JsonSerializer.Deserialize<PaginationMetadata>(metadataHeader, serializerOptions);
        if (metadata is null)
        {
            return Result<PaginationScheme<OrderScheme>>.Failure(CommonErrors.InvalidContent);
        }

        var items = JsonSerializer.Deserialize<IEnumerable<OrderScheme>>(content, serializerOptions);
        if (items is null)
        {
            return Result<PaginationScheme<OrderScheme>>.Failure(CommonErrors.InvalidContent);
        }

        var result = new PaginationScheme<OrderScheme>
        {
            Items = [.. items],
            Total = metadata.Total,
            PageNumber = metadata.PageNumber,
            PageSize = metadata.PageSize
        };

        return Result<PaginationScheme<OrderScheme>>.Success(result);
    }

    public async Task<Result<OrderScheme>> CreateOrderAsync(OrderCreationScheme parameters, CancellationToken cancellation = default)
    {
        var response = await httpClient.PostAsJsonAsync("orders", parameters, cancellation);
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
            return Result<OrderScheme>.Failure(error);
        }

        var order = JsonSerializer.Deserialize<OrderScheme>(content, serializerOptions);
        if (order is null)
        {
            return Result<OrderScheme>.Failure(CommonErrors.InvalidContent);
        }

        return Result<OrderScheme>.Success(order);
    }

    public async Task<Result<OrderScheme>> UpdateOrderAsync(
        OrderModificationScheme parameters, CancellationToken cancellation = default)
    {
        var response = await httpClient.PutAsJsonAsync($"orders/{parameters.Id}", parameters, cancellation);
        var content = await response.Content.ReadAsStringAsync(cancellation);

        var error = response.StatusCode switch
        {
            HttpStatusCode.NotFound => OrderErrors.OrderDoesNotExist,
            HttpStatusCode.Unauthorized => CommonErrors.UnauthorizedAccess,
            HttpStatusCode.Forbidden => CommonErrors.UnauthorizedAccess,
            HttpStatusCode.InternalServerError => CommonErrors.OperationFailed,

            _ => null
        };

        if (error is not null)
        {
            return Result<OrderScheme>.Failure(error);
        }

        var order = JsonSerializer.Deserialize<OrderScheme>(content, serializerOptions);
        if (order is null)
        {
            return Result<OrderScheme>.Failure(CommonErrors.InvalidContent);
        }

        return Result<OrderScheme>.Success(order);
    }
}
