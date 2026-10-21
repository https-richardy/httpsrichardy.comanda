namespace Comanda.Internal.Contracts.Tests.Integration.Clients;

public sealed class AuthenticationClientTests(WireMockFixture fixture) :
    IClassFixture<WireMockFixture>,
    IAsyncLifetime
{
    private readonly Fixture _fixture = new();
    private readonly JsonSerializerOptions _serializer = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        PropertyNameCaseInsensitive = true,
    };

    public ValueTask InitializeAsync() => ValueTask.CompletedTask;
    public ValueTask DisposeAsync()
    {
        fixture.WireMock.Reset();
        return ValueTask.CompletedTask;
    }

    [Fact(DisplayName = "[integration] - when AuthenticateAsync is called with valid client credentials, a successful result with the access token is returned")]
    public async Task When_AuthenticateAsync_IsCalled_WithValidCredentials_Then_SuccessfulResultIsReturned()
    {
        /* arrange: configures wiremock stub simulating the identity provider token endpoint */
        var credentials = _fixture.Build<ClientCredentials>()
            .Create();

        var authentication = _fixture.Build<AuthenticationScheme>()
            .With(scheme => scheme.TokenType, "Bearer")
            .Create();

        fixture.WireMock.Given(Request.Create().UsingPost()
            .WithPath("/protocol/openid-connect/token"))
            .RespondWith(Response.Create()
            .WithStatusCode(HttpStatusCode.OK)
            .WithHeader(HeaderNames.ContentType, MediaTypeNames.Application.Json)
            .WithBody(JsonSerializer.Serialize(authentication, _serializer)));

        /* act: authenticate through the client */
        var client = new AuthenticationClient(fixture.HttpClient);
        var result = await client.AuthenticateAsync(credentials, TestContext.Current.CancellationToken);

        /* assert: validate the deserialized authentication scheme */
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.Equal(authentication.AccessToken, result.Data.AccessToken);
        Assert.Equal(authentication.TokenType, result.Data.TokenType);

        /* assert: verify credentials were actually sent using the client credentials grant type */
        var requests = fixture.WireMock.FindLogEntries(
        [
            Request.Create().UsingPost().WithPath("/protocol/openid-connect/token")
        ]);

        var request = Assert.Single(requests);
        var body = request.RequestMessage?.Body ?? string.Empty;

        Assert.Contains("grant_type=client_credentials", body);
        Assert.Contains($"client_id={credentials.ClientId}", body);
    }

    [Fact(DisplayName = "[integration] - when AuthenticateAsync is called and the identity provider rejects the credentials, a failure result is returned")]
    public async Task When_AuthenticateAsync_IsCalled_AndIdentityProviderRejectsCredentials_Then_FailureResultIsReturned()
    {
        /* arrange: configures wiremock stub simulating invalid client credentials */
        var credentials = _fixture.Build<ClientCredentials>()
            .Create();

        fixture.WireMock.Given(Request.Create().UsingPost()
            .WithPath("/protocol/openid-connect/token"))
            .RespondWith(Response.Create()
            .WithStatusCode(HttpStatusCode.Unauthorized));

        /* act: authenticate through the client */
        var client = new AuthenticationClient(fixture.HttpClient);
        var result = await client.AuthenticateAsync(credentials, TestContext.Current.CancellationToken);

        /* assert: validate the returned failure */
        Assert.True(result.IsFailure);
        Assert.Equal(Error.Unknown, result.Error);
    }
}
