namespace Comanda.Internal.Contracts.Clients;

public sealed class SubscriptionClient(HttpClient httpClient) : ISubscriptionClient
{
    private readonly JsonSerializerOptions serializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true
    };

    public async Task<Result<SubscriptionCheckoutSession>> CreateCheckoutSessionAsync(
        SubscriptionCheckoutSessionCreationScheme parameters, CancellationToken cancellation = default)
    {
        var response = await httpClient.PostAsJsonAsync("subscriptions", parameters, cancellation);
        var content = await response.Content.ReadAsStringAsync(cancellation);

        var error = response.StatusCode switch
        {
            HttpStatusCode.Unauthorized => CommonErrors.UnauthorizedAccess,
            HttpStatusCode.Forbidden => CommonErrors.UnauthorizedAccess,
            HttpStatusCode.UnprocessableEntity => SubscriptionErrors.PlanNotSupported,
            HttpStatusCode.InternalServerError => CommonErrors.OperationFailed,

            _ => null
        };

        if (error is not null)
        {
            return Result<SubscriptionCheckoutSession>.Failure(error);
        }

        var session = JsonSerializer.Deserialize<SubscriptionCheckoutSession>(content, serializerOptions);
        if (session is null)
        {
            return Result<SubscriptionCheckoutSession>.Failure(CommonErrors.InvalidContent);
        }

        return Result<SubscriptionCheckoutSession>.Success(session);
    }

    public async Task<Result<SubscriptionScheme>> ProcessSuccessfulCheckoutAsync(
        CallbackSuccessfulCheckoutParameters parameters, CancellationToken cancellation = default)
    {
        var queryString = QueryParametersParser.ToQueryString(parameters);

        var response = await httpClient.GetAsync($"callback/success?{queryString}", cancellation);
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
            return Result<SubscriptionScheme>.Failure(error);
        }

        var subscription = JsonSerializer.Deserialize<SubscriptionScheme>(content, serializerOptions);
        if (subscription is null)
        {
            return Result<SubscriptionScheme>.Failure(CommonErrors.InvalidContent);
        }

        return Result<SubscriptionScheme>.Success(subscription);
    }

    public async Task<Result<SubscriptionScheme>> CancelSubscriptionAsync(
        SubscriptionCancelationScheme parameters, CancellationToken cancellation = default)
    {
        var response = await httpClient.DeleteAsync($"subscriptions/{parameters.SubscriptionId}", cancellation);
        var content = await response.Content.ReadAsStringAsync(cancellation);

        var error = response.StatusCode switch
        {
            HttpStatusCode.Unauthorized => CommonErrors.UnauthorizedAccess,
            HttpStatusCode.Forbidden => CommonErrors.UnauthorizedAccess,
            HttpStatusCode.InternalServerError => CommonErrors.OperationFailed,

            HttpStatusCode.NotFound => SubscriptionErrors.SubscriptionDoesNotExist,
            HttpStatusCode.UnprocessableEntity => SubscriptionErrors.SubscriptionAlreadyCanceled,

            _ => null
        };

        if (error is not null)
        {
            return Result<SubscriptionScheme>.Failure(error);
        }

        var subscription = JsonSerializer.Deserialize<SubscriptionScheme>(content, serializerOptions);
        if (subscription is null)
        {
            return Result<SubscriptionScheme>.Failure(CommonErrors.InvalidContent);
        }

        return Result<SubscriptionScheme>.Success(subscription);
    }

    public async Task<Result<SubscriptionScheme>> ProcessFailedCheckoutAsync(
        CallbackFailedCheckoutParameters parameters, CancellationToken cancellation = default)
    {
        var queryString = QueryParametersParser.ToQueryString(parameters);

        var response = await httpClient.GetAsync($"callback/cancel?{queryString}", cancellation);
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
            return Result<SubscriptionScheme>.Failure(error);
        }

        var subscription = JsonSerializer.Deserialize<SubscriptionScheme>(content, serializerOptions);
        if (subscription is null)
        {
            return Result<SubscriptionScheme>.Failure(CommonErrors.InvalidContent);
        }

        return Result<SubscriptionScheme>.Success(subscription);
    }
}
