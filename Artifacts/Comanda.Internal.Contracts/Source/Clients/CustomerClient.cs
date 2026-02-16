namespace Comanda.Internal.Contracts.Clients;

public sealed class CustomerClient(HttpClient httpClient) : ICustomerClient
{
    private readonly JsonSerializerOptions serializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true
    };

    public async Task<Result<CustomerScheme>> CreateCustomerAsync(
        CustomerCreationScheme parameters, CancellationToken cancellation = default)
    {
        var response = await httpClient.PostAsJsonAsync("customers", parameters, cancellation);
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
            return Result<CustomerScheme>.Failure(error);
        }

        var customer = JsonSerializer.Deserialize<CustomerScheme>(content, serializerOptions);
        if (customer is null)
        {
            return Result<CustomerScheme>.Failure(CommonErrors.InvalidContent);
        }

        return Result<CustomerScheme>.Success(customer);
    }

    public async Task<Result<PaginationScheme<CustomerScheme>>> GetCustomersAsync(
        FetchCustomersParameters parameters, CancellationToken cancellation = default)
    {
        var queryString = QueryParametersParser.ToQueryString(parameters);

        var response = await httpClient.GetAsync($"customers?{queryString}", cancellation);
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
            return Result<PaginationScheme<CustomerScheme>>.Failure(error);
        }

        var metadataHeader = response.Headers
            .GetValues("X-Pagination")
            .FirstOrDefault();

        if (metadataHeader is null)
        {
            return Result<PaginationScheme<CustomerScheme>>.Failure(CommonErrors.InvalidContent);
        }

        var metadata = JsonSerializer.Deserialize<PaginationMetadata>(metadataHeader, serializerOptions);
        if (metadata is null)
        {
            return Result<PaginationScheme<CustomerScheme>>.Failure(CommonErrors.InvalidContent);
        }

        var items = JsonSerializer.Deserialize<IEnumerable<CustomerScheme>>(content, serializerOptions);
        if (items is null)
        {
            return Result<PaginationScheme<CustomerScheme>>.Failure(CommonErrors.InvalidContent);
        }

        var result = new PaginationScheme<CustomerScheme>
        {
            Items = [.. items],
            Total = metadata.Total,
            PageNumber = metadata.PageNumber,
            PageSize = metadata.PageSize
        };

        return Result<PaginationScheme<CustomerScheme>>.Success(result);
    }

    public async Task<Result<CustomerScheme>> EditCustomerAsync(
        EditCustomerScheme parameters, CancellationToken cancellation = default)
    {
        var response = await httpClient.PutAsJsonAsync($"customers/{parameters.CustomerId}", parameters, cancellation);
        var content = await response.Content.ReadAsStringAsync(cancellation);

        var error = response.StatusCode switch
        {
            HttpStatusCode.NotFound => CustomerErrors.CustomerDoesNotExist,
            HttpStatusCode.Unauthorized => CommonErrors.UnauthorizedAccess,
            HttpStatusCode.Forbidden => CommonErrors.UnauthorizedAccess,
            HttpStatusCode.InternalServerError => CommonErrors.OperationFailed,

            _ => null
        };

        if (error is not null)
        {
            return Result<CustomerScheme>.Failure(error);
        }

        var customer = JsonSerializer.Deserialize<CustomerScheme>(content, serializerOptions);
        if (customer is null)
        {
            return Result<CustomerScheme>.Failure(CommonErrors.InvalidContent);
        }

        return Result<CustomerScheme>.Success(customer);
    }

    public async Task<Result> DeleteCustomerAsync(
        CustomerDeletionScheme parameters, CancellationToken cancellation = default)
    {
        var response = await httpClient.DeleteAsync($"customers/{parameters.CustomerId}", cancellation);
        var error = response.StatusCode switch
        {
            HttpStatusCode.NotFound => CustomerErrors.CustomerDoesNotExist,
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

    public async Task<Result<IReadOnlyCollection<Address>>> GetCustomerAddressAsync(
        FetchCustomerAddressesParameters parameters, CancellationToken cancellation = default)
    {
        var queryString = QueryParametersParser.ToQueryString(parameters);

        var response = await httpClient.GetAsync($"customers/{parameters.CustomerId}/addresses", cancellation);
        var content = await response.Content.ReadAsStringAsync(cancellation);

        var error = response.StatusCode switch
        {
            HttpStatusCode.NotFound => CustomerErrors.CustomerDoesNotExist,
            HttpStatusCode.Unauthorized => CommonErrors.UnauthorizedAccess,
            HttpStatusCode.Forbidden => CommonErrors.UnauthorizedAccess,
            HttpStatusCode.InternalServerError => CommonErrors.OperationFailed,

            _ => null
        };

        if (error is not null)
        {
            return Result<IReadOnlyCollection<Address>>.Failure(error);
        }

        var addresses = JsonSerializer.Deserialize<IReadOnlyCollection<Address>>(content, serializerOptions);
        if (addresses is null)
        {
            return Result<IReadOnlyCollection<Address>>.Failure(CommonErrors.InvalidContent);
        }

        return Result<IReadOnlyCollection<Address>>.Success(addresses);
    }

    public async Task<Result<Address>> AssignCustomerAddressAsync(
        AssignCustomerAddressScheme parameters, CancellationToken cancellation = default)
    {
        var response = await httpClient.PostAsJsonAsync($"customers/{parameters.CustomerId}/addresses", parameters, cancellation);
        var content = await response.Content.ReadAsStringAsync(cancellation);

        var error = response.StatusCode switch
        {
            HttpStatusCode.NotFound => CustomerErrors.CustomerDoesNotExist,
            HttpStatusCode.Conflict => CustomerErrors.AddressAlreadyAssigned,

            HttpStatusCode.Unauthorized => CommonErrors.UnauthorizedAccess,
            HttpStatusCode.Forbidden => CommonErrors.UnauthorizedAccess,
            HttpStatusCode.InternalServerError => CommonErrors.OperationFailed,

            _ => null
        };

        if (error is not null)
        {
            return Result<Address>.Failure(error);
        }

        var address = JsonSerializer.Deserialize<Address>(content, serializerOptions);
        if (address is null)
        {
            return Result<Address>.Failure(CommonErrors.InvalidContent);
        }

        return Result<Address>.Success(address);
    }

    public async Task<Result<Address>> EditCustomerAddressAsync(
        EditCustomerAddressScheme parameters, CancellationToken cancellation = default)
    {
        var response = await httpClient.PutAsJsonAsync($"customers/{parameters.CustomerId}/addresses", parameters, cancellation);
        var content = await response.Content.ReadAsStringAsync(cancellation);

        if (!response.IsSuccessStatusCode)
        {
            var error = JsonSerializer.Deserialize<Error>(content, serializerOptions);
            if (error is not null)
            {
                return Result<Address>.Failure(error);
            }

            return Result<Address>.Failure(CommonErrors.OperationFailed);
        }

        var address = JsonSerializer.Deserialize<Address>(content, serializerOptions);
        if (address is null)
        {
            return Result<Address>.Failure(CommonErrors.InvalidContent);
        }

        return Result<Address>.Success(address);
    }

    public async Task<Result> DeleteCustomerAddressAsync(
        DeleteCustomerAddressScheme parameters, CancellationToken cancellation = default)
    {
        var response = await httpClient.DeleteAsync($"customers/{parameters.CustomerId}/addresses", cancellation);
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
