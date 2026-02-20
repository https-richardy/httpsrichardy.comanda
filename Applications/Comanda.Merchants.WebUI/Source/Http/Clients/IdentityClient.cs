#pragma warning disable S1125 // we prefer explicit boolean comparisons for readability

namespace Comanda.Merchants.WebUI.Http.Clients;

public sealed class IdentityClient(HttpClient httpClient) : IIdentityClient
{
    private readonly JsonSerializerOptions serializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public async Task<Result<AuthenticationResult>> AuthenticateAsync(AuthenticationCredentials credentials, CancellationToken cancellation = default)
    {
        var response = await httpClient.PostAsJsonAsync("api/v1/identity/authenticate", credentials, cancellation);
        if (response.IsSuccessStatusCode is false)
        {
            var error = await response.Content.ReadFromJsonAsync<Error>(
                options: serializerOptions,
                cancellationToken: cancellation
            );

            return error is not null
                ? Result<AuthenticationResult>.Failure(error)
                : Result<AuthenticationResult>.Failure(Error.Unknown);
        }

        var result = await response.Content.ReadFromJsonAsync<AuthenticationResult>(
            options: serializerOptions,
            cancellationToken: cancellation
        );

        return result is not null
            ? Result<AuthenticationResult>.Success(result)
            : Result<AuthenticationResult>.Failure(Error.Unknown);
    }

    public async Task<Result<UserDetails>> CreateIdentityAsync(IdentityEnrollmentCredentials credentials, CancellationToken cancellation = default)
    {
        var response = await httpClient.PostAsJsonAsync("api/v1/identity", credentials, cancellation);
        if (response.IsSuccessStatusCode is false)
        {
            var error = await response.Content.ReadFromJsonAsync<Error>(
                options: serializerOptions,
                cancellationToken: cancellation
            );

            return error is not null
                ? Result<UserDetails>.Failure(error)
                : Result<UserDetails>.Failure(Error.Unknown);
        }

        var result = await response.Content.ReadFromJsonAsync<UserDetails>(
            options: serializerOptions,
            cancellationToken: cancellation
        );

        return result is not null
            ? Result<UserDetails>.Success(result)
            : Result<UserDetails>.Failure(Error.Unknown);
    }

    public async Task<Result> InvalidateSessionAsync(SessionInvalidation session, CancellationToken cancellation = default)
    {
        var response = await httpClient.PostAsJsonAsync("api/v1/identity/invalidate-session", session, cancellation);
        if (response.IsSuccessStatusCode is false)
        {
            var error = await response.Content.ReadFromJsonAsync<Error>(
                options: serializerOptions,
                cancellationToken: cancellation
            );

            return error is not null
                ? Result.Failure(error)
                : Result.Failure(Error.Unknown);
        }

        return Result.Success();
    }
}
