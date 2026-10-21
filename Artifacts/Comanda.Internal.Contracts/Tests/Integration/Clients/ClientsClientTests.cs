namespace Comanda.Internal.Contracts.Tests.Integration.Clients;

public sealed class ClientsClientTests(WireMockFixture fixture) : IClassFixture<WireMockFixture>
{
    private readonly Fixture _fixture = new();
    private readonly JsonSerializerOptions _serializer = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
    };

    [Fact(DisplayName = "[integration] - when GetClientsAsync is called and keycloak responds with clients, a successful result with the paginated clients is returned")]
    public async Task When_GetClientsAsync_IsCalled_AndKeycloakRespondsWithClients_Then_SuccessfulResultIsReturned()
    {
        /* arrange: configures wiremock stub simulating the keycloak clients listing endpoint */
        var clientId = _fixture.Create<string>();
        var parameters = new ClientFiltersParameters().WithClientId(clientId);

        var clients = _fixture.CreateMany<ClientScheme>(2).ToList();

        fixture.WireMock.Given(Request.Create().UsingGet()
            .WithPath("/clients")
            .WithParam("clientId", clientId))
            .RespondWith(Response.Create()
            .WithStatusCode(HttpStatusCode.OK)
            .WithHeader(HeaderNames.ContentType, MediaTypeNames.Application.Json)
            .WithBody(JsonSerializer.Serialize(clients, _serializer)));

        /* act: request clients through the client */
        var client = new ClientsClient(fixture.HttpClient);
        var result = await client.GetClientsAsync(parameters, TestContext.Current.CancellationToken);

        /* assert: validate the manually assembled pagination */
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.Equal(clients.Count, result.Data.Items.Count);
    }

    [Fact(DisplayName = "[integration] - when GetClientsAsync is called and keycloak responds with an error, a failure result with the unknown error is returned")]
    public async Task When_GetClientsAsync_IsCalled_AndKeycloakRespondsWithError_Then_FailureResultIsReturned()
    {
        /* arrange: configures wiremock stub simulating a keycloak failure */
        var clientId = _fixture.Create<string>();
        var parameters = new ClientFiltersParameters().WithClientId(clientId);

        fixture.WireMock.Given(Request.Create().UsingGet()
            .WithPath("/clients")
            .WithParam("clientId", clientId))
            .RespondWith(Response.Create()
            .WithStatusCode(HttpStatusCode.InternalServerError));

        /* act: request clients through the client */
        var client = new ClientsClient(fixture.HttpClient);
        var result = await client.GetClientsAsync(parameters, TestContext.Current.CancellationToken);

        /* assert: validate the returned failure */
        Assert.True(result.IsFailure);
        Assert.Equal(Error.Unknown, result.Error);
    }

    [Fact(DisplayName = "[integration] - when GetClientAsync is called with an existing client id, a successful result with the client is returned")]
    public async Task When_GetClientAsync_IsCalled_WithExistingClientId_Then_SuccessfulResultIsReturned()
    {
        /* arrange: configures wiremock stub simulating an existing keycloak client */
        var client = _fixture.Create<ClientScheme>();

        fixture.WireMock.Given(Request.Create().UsingGet()
            .WithPath($"/clients/{client.Id}"))
            .RespondWith(Response.Create()
            .WithStatusCode(HttpStatusCode.OK)
            .WithHeader(HeaderNames.ContentType, MediaTypeNames.Application.Json)
            .WithBody(JsonSerializer.Serialize(client, _serializer)));

        /* act: request the client through the client sdk */
        var sut = new ClientsClient(fixture.HttpClient);
        var result = await sut.GetClientAsync(client.Id, TestContext.Current.CancellationToken);

        /* assert: validate the deserialized client */
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.Equal(client.Id, result.Data.Id);
        Assert.Equal(client.ClientId, result.Data.ClientId);
    }

    [Fact(DisplayName = "[integration] - when GetClientAsync is called with a non-existing client id, a failure result is returned")]
    public async Task When_GetClientAsync_IsCalled_WithNonExistingClientId_Then_FailureResultIsReturned()
    {
        /* arrange: configures wiremock stub simulating a missing keycloak client */
        var clientId = _fixture.Create<string>();

        fixture.WireMock.Given(Request.Create().UsingGet()
            .WithPath($"/clients/{clientId}"))
            .RespondWith(Response.Create()
            .WithStatusCode(HttpStatusCode.NotFound));

        /* act: request the client through the client sdk */
        var sut = new ClientsClient(fixture.HttpClient);
        var result = await sut.GetClientAsync(clientId, TestContext.Current.CancellationToken);

        /* assert: validate the returned failure */
        Assert.True(result.IsFailure);
        Assert.Equal(AuthorizationErrors.ClientDoesNotExist, result.Error);
    }
}
