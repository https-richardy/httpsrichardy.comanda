namespace Comanda.Internal.Contracts.Clients;

public sealed class PaymentClient(HttpClient httpClient) : IPaymentClient
{
    private readonly JsonSerializerOptions serializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true
    };

    public async Task<Result<PaginationScheme<PaymentScheme>>> GetPaymentsAsync(
        PaymentsFetchParameters parameters, CancellationToken cancellation = default)
    {
        var queryString = QueryParametersParser.ToQueryString(parameters);

        var response = await httpClient.GetAsync($"payments?{queryString}", cancellation);
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
            return Result<PaginationScheme<PaymentScheme>>.Failure(error);
        }

        var metadataHeader = response.Headers
            .GetValues("X-Pagination")
            .FirstOrDefault();

        if (metadataHeader is null)
        {
            return Result<PaginationScheme<PaymentScheme>>.Failure(CommonErrors.InvalidContent);
        }

        var metadata = JsonSerializer.Deserialize<PaginationMetadata>(metadataHeader, serializerOptions);
        if (metadata is null)
        {
            return Result<PaginationScheme<PaymentScheme>>.Failure(CommonErrors.InvalidContent);
        }

        var items = JsonSerializer.Deserialize<IEnumerable<PaymentScheme>>(content, serializerOptions);
        if (items is null)
        {
            return Result<PaginationScheme<PaymentScheme>>.Failure(CommonErrors.InvalidContent);
        }

        var result = new PaginationScheme<PaymentScheme>
        {
            Items = [.. items],
            Total = metadata.Total,
            PageNumber = metadata.PageNumber,
            PageSize = metadata.PageSize
        };

        return Result<PaginationScheme<PaymentScheme>>.Success(result);
    }

    public async Task<Result<PaymentScheme>> CreateLocalPaymentAsync(
        OfflinePaymentScheme parameters, CancellationToken cancellation = default)
    {
        var response = await httpClient.PostAsJsonAsync("payments/offline", parameters, cancellation);
        var content = await response.Content.ReadAsStringAsync(cancellation);

        var error = response.StatusCode switch
        {
            HttpStatusCode.UnprocessableEntity => PaymentErrors.MethodNotAllowed,
            HttpStatusCode.Unauthorized => CommonErrors.UnauthorizedAccess,
            HttpStatusCode.Forbidden => CommonErrors.UnauthorizedAccess,
            HttpStatusCode.InternalServerError => CommonErrors.OperationFailed,

            _ => null
        };

        if (error is not null)
        {
            return Result<PaymentScheme>.Failure(error);
        }

        var payment = JsonSerializer.Deserialize<PaymentScheme>(content, serializerOptions);
        if (payment is null)
        {
            return Result<PaymentScheme>.Failure(CommonErrors.InvalidContent);
        }

        return Result<PaymentScheme>.Success(payment);
    }

    public async Task<Result<CheckoutSession>> CreateOnlineChargeAsync(
        CheckoutSessionCreationScheme parameters, Credential credential, CancellationToken cancellation = default)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "payments/online")
        {
            Content = JsonContent.Create(parameters)
        };

        // each establishment will register its own secret key for the payment gateway.
        // therefore, whenever we process payments on the payment gateway, we must send the credential via the header.

        request.Headers.Add("X-Credential", credential.SecretKey);

        var response = await httpClient.SendAsync(request, cancellation);
        var content = await response.Content.ReadAsStringAsync(cancellation);

        var error = response.StatusCode switch
        {
            HttpStatusCode.Unauthorized => CommonErrors.UnauthorizedAccess,
            HttpStatusCode.Forbidden => CommonErrors.UnauthorizedAccess,
            HttpStatusCode.InternalServerError => CommonErrors.OperationFailed,
            HttpStatusCode.BadGateway => AbacatePayErrors.OperationFailed,

            _ => null
        };

        if (error is not null)
        {
            return Result<CheckoutSession>.Failure(error);
        }

        var session = JsonSerializer.Deserialize<CheckoutSession>(content, serializerOptions);
        if (session is null)
        {
            return Result<CheckoutSession>.Failure(CommonErrors.InvalidContent);
        }

        return Result<CheckoutSession>.Success(session);
    }

    public async Task<Result<PaymentScheme>> UpdatePaymentStatusAsync(PaymentStatusUpdateScheme parameters, CancellationToken cancellation = default)
    {
        var response = await httpClient.PutAsJsonAsync($"payments/{parameters.Identifier}/status", parameters, cancellation);
        var content = await response.Content.ReadAsStringAsync(cancellation);

        var error = response.StatusCode switch
        {
            HttpStatusCode.UnprocessableEntity => PaymentErrors.MethodNotAllowed,
            HttpStatusCode.NotFound => PaymentErrors.PaymentDoesNotExist,

            HttpStatusCode.Unauthorized => CommonErrors.UnauthorizedAccess,
            HttpStatusCode.Forbidden => CommonErrors.UnauthorizedAccess,
            HttpStatusCode.InternalServerError => CommonErrors.OperationFailed,

            _ => null
        };

        if (error is not null)
        {
            return Result<PaymentScheme>.Failure(error);
        }

        var payment = JsonSerializer.Deserialize<PaymentScheme>(content, serializerOptions);
        if (payment is null)
        {
            return Result<PaymentScheme>.Failure(CommonErrors.InvalidContent);
        }

        return Result<PaymentScheme>.Success(payment);
    }
}
