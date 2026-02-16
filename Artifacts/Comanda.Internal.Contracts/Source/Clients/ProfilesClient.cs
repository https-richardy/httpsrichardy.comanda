namespace Comanda.Internal.Contracts.Clients;

public sealed class ProfilesClient(HttpClient httpClient) : IProfilesClient
{
    private readonly JsonSerializerOptions serializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true
    };

    public async Task<Result<PaginationScheme<OwnerScheme>>> GetOwnersAsync(
        FetchOwnersParameters parameters, CancellationToken cancellation = default)
    {
        var queryString = QueryParametersParser.ToQueryString(parameters);

        var response = await httpClient.GetAsync($"profiles/owners?{queryString}", cancellation);
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

    public async Task<Result<OwnerScheme>> CreateOwnerAsync(
        OwnerCreationScheme parameters, CancellationToken cancellation = default)
    {
        var response = await httpClient.PostAsJsonAsync("profiles/owners", parameters, cancellation);
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

    public async Task<Result<OwnerScheme>> UpdateOwnerAsync(
        EditOwnerScheme parameters, CancellationToken cancellation = default)
    {
        var response = await httpClient.PutAsJsonAsync($"profiles/owners/{parameters.OwnerId}", parameters, cancellation);
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
        var response = await httpClient.DeleteAsync($"profiles/owners/{parameters.OwnerId}", cancellation);
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

    public async Task<Result<PaginationScheme<CustomerScheme>>> GetCustomersAsync(
        FetchCustomersParameters parameters, CancellationToken cancellation = default)
    {
        var queryString = QueryParametersParser.ToQueryString(parameters);

        var response = await httpClient.GetAsync($"profiles/customers?{queryString}", cancellation);
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

    public async Task<Result<CustomerScheme>> CreateCustomerAsync(
        CustomerCreationScheme parameters, CancellationToken cancellation = default)
    {
        var response = await httpClient.PostAsJsonAsync("profiles/customers", parameters, cancellation);
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

    public async Task<Result<CustomerScheme>> EditCustomerAsync(
        EditCustomerScheme parameters, CancellationToken cancellation = default)
    {
        var response = await httpClient.PutAsJsonAsync($"profiles/customers/{parameters.CustomerId}", parameters, cancellation);
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
        var response = await httpClient.DeleteAsync($"profiles/customers/{parameters.CustomerId}", cancellation);
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
        var response = await httpClient.GetAsync($"profiles/customers/{parameters.CustomerId}/addresses", cancellation);
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
        var response = await httpClient.PostAsJsonAsync($"profiles/customers/{parameters.CustomerId}/addresses", parameters, cancellation);
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
        var response = await httpClient.PutAsJsonAsync($"profiles/customers/{parameters.CustomerId}/addresses", parameters, cancellation);
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
        var response = await httpClient.DeleteAsync($"profiles/customers/{parameters.CustomerId}/addresses", cancellation);
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
