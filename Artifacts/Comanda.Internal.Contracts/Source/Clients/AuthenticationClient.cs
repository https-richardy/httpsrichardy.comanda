namespace Comanda.Internal.Contracts.Clients;

public sealed class AuthenticationClient(HttpClient httpClient) : IAuthenticationClient
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly JsonSerializerOptions _serializer = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        PropertyNameCaseInsensitive = true
    };

    public async Task<Result<AuthenticationScheme>> AuthenticateAsync(ClientCredentials credentials, CancellationToken cancellation = default)
    {
        // https://www.rfc-editor.org/info/rfc6749/#section-4.4.2
        // use client credentials grant type for service-to-service authentication

        var content = new FormUrlEncodedContent(
        [
            new("grant_type", "client_credentials"),
            new("client_id", credentials.ClientId),
            new("client_secret", credentials.ClientSecret)
        ]);

        var response = await _httpClient.PostAsync("protocol/openid-connect/token", content, cancellation);
        var contentString = await response.Content.ReadAsStringAsync(cancellation);

        if (!response.IsSuccessStatusCode)
        {
            return Result<AuthenticationScheme>.Failure(Error.Unknown);
        }

        var authentication = JsonSerializer.Deserialize<AuthenticationScheme>(contentString, _serializer);

        return authentication is not null
            ? Result<AuthenticationScheme>.Success(authentication)
            : Result<AuthenticationScheme>.Failure(Error.Unknown);
    }
}
