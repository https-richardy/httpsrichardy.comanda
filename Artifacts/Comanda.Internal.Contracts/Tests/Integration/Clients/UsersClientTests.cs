namespace Comanda.Internal.Contracts.Tests.Integration.Clients;

public sealed class UsersClientTests(WireMockFixture fixture) : IClassFixture<WireMockFixture>
{
    private readonly Fixture _fixture = new();
    private readonly JsonSerializerOptions _serializer = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
    };

    [Fact(DisplayName = "[integration] - when GetUsersAsync is called and the identity provider responds with users, a successful result with the paginated users is returned")]
    public async Task When_GetUsersAsync_IsCalled_AndIdentityProviderRespondsWithUsers_Then_SuccessfulResultIsReturned()
    {
        /* arrange: configures wiremock stub simulating the keycloak users listing endpoint */
        var username = _fixture.Create<string>();
        var parameters = new UserFiltersParameters().WithUsername(username);

        var users = _fixture.CreateMany<UserScheme>(2).ToList();

        fixture.WireMock.Given(Request.Create().UsingGet()
            .WithPath("/users")
            .WithParam("username", username))
            .RespondWith(Response.Create()
            .WithStatusCode(HttpStatusCode.OK)
            .WithHeader(HeaderNames.ContentType, MediaTypeNames.Application.Json)
            .WithBody(JsonSerializer.Serialize(users, _serializer)));

        /* act: request users through the client */
        var client = new UsersClient(fixture.HttpClient);
        var result = await client.GetUsersAsync(parameters, TestContext.Current.CancellationToken);

        /* assert: validate the manually assembled pagination */
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.Equal(users.Count, result.Data.Items.Count);
        Assert.Equal(parameters.Page, result.Data.PageNumber);
        Assert.Equal(parameters.PageSize, result.Data.PageSize);
    }

    [Fact(DisplayName = "[integration] - when GetUsersAsync is called and the identity provider responds with an error, a failure result with the unknown error is returned")]
    public async Task When_GetUsersAsync_IsCalled_AndIdentityProviderRespondsWithError_Then_FailureResultIsReturned()
    {
        /* arrange: configures wiremock stub simulating an identity provider failure */
        var username = _fixture.Create<string>();
        var parameters = new UserFiltersParameters().WithUsername(username);

        fixture.WireMock.Given(Request.Create().UsingGet()
            .WithPath("/users")
            .WithParam("username", username))
            .RespondWith(Response.Create()
            .WithStatusCode(HttpStatusCode.InternalServerError));

        /* act: request users through the client */
        var client = new UsersClient(fixture.HttpClient);
        var result = await client.GetUsersAsync(parameters, TestContext.Current.CancellationToken);

        /* assert: validate the returned failure */
        Assert.True(result.IsFailure);
        Assert.Equal(Error.Unknown, result.Error);
    }

    [Fact(DisplayName = "[integration] - when GetUserAsync is called with an existing user id, a successful result with the user is returned")]
    public async Task When_GetUserAsync_IsCalled_WithExistingUserId_Then_SuccessfulResultIsReturned()
    {
        /* arrange: configures wiremock stub simulating an existing keycloak user */
        var user = _fixture.Create<UserScheme>();

        fixture.WireMock.Given(Request.Create().UsingGet()
            .WithPath($"/users/{user.Id}"))
            .RespondWith(Response.Create()
            .WithStatusCode(HttpStatusCode.OK)
            .WithHeader(HeaderNames.ContentType, MediaTypeNames.Application.Json)
            .WithBody(JsonSerializer.Serialize(user, _serializer)));

        /* act: request the user through the client */
        var client = new UsersClient(fixture.HttpClient);
        var result = await client.GetUserAsync(user.Id, TestContext.Current.CancellationToken);

        /* assert: validate the deserialized user */
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.Equal(user.Id, result.Data.Id);
        Assert.Equal(user.Username, result.Data.Username);
    }

    [Fact(DisplayName = "[integration] - when GetUserAsync is called with a non-existing user id, a failure result is returned")]
    public async Task When_GetUserAsync_IsCalled_WithNonExistingUserId_Then_FailureResultIsReturned()
    {
        /* arrange: configures wiremock stub simulating a missing keycloak user */
        var userId = _fixture.Create<string>();

        fixture.WireMock.Given(Request.Create().UsingGet()
            .WithPath($"/users/{userId}"))
            .RespondWith(Response.Create()
            .WithStatusCode(HttpStatusCode.NotFound));

        /* act: request the user through the client */
        var client = new UsersClient(fixture.HttpClient);
        var result = await client.GetUserAsync(userId, TestContext.Current.CancellationToken);

        /* assert: validate the returned failure */
        Assert.True(result.IsFailure);
        Assert.Equal(AuthorizationErrors.UserDoesNotExist, result.Error);
    }

    [Fact(DisplayName = "[integration] - when CreateUserAsync is called with a valid request, a successful result with the created user is returned")]
    public async Task When_CreateUserAsync_IsCalled_WithValidRequest_Then_SuccessfulResultIsReturned()
    {
        /* arrange: resolves the client and builds a user creation request */
        var request = _fixture.Build<UserCreationScheme>()
            .Create();

        var userId = Guid.NewGuid().ToString("N");
        var user = _fixture.Build<UserScheme>()
            .With(scheme => scheme.Id, userId)
            .With(scheme => scheme.Username, request.Username)
            .Create();

        /* arrange: configures wiremock stubs simulating keycloak user creation followed by the location lookup */
        fixture.WireMock.Given(Request.Create().UsingPost()
            .WithPath("/users"))
            .RespondWith(Response.Create()
            .WithStatusCode(HttpStatusCode.Created)
            .WithHeader("Location", $"{fixture.WireMock.Url}/users/{userId}"));

        fixture.WireMock.Given(Request.Create().UsingGet()
            .WithPath($"/users/{userId}"))
            .RespondWith(Response.Create()
            .WithStatusCode(HttpStatusCode.OK)
            .WithHeader(HeaderNames.ContentType, MediaTypeNames.Application.Json)
            .WithBody(JsonSerializer.Serialize(user, _serializer)));

        /* act: create the user through the client */
        var client = new UsersClient(fixture.HttpClient);
        var result = await client.CreateUserAsync(request, TestContext.Current.CancellationToken);

        /* assert: validate the user was fetched back through the location header */
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.Equal(userId, result.Data.Id);
        Assert.Equal(request.Username, result.Data.Username);
    }

    [Fact(DisplayName = "[integration] - when CreateUserAsync is called and the identity provider rejects the request, a failure result is returned")]
    public async Task When_CreateUserAsync_IsCalled_AndIdentityProviderRejectsRequest_Then_FailureResultIsReturned()
    {
        /* arrange: builds a user creation request that will be rejected */
        var request = _fixture.Build<UserCreationScheme>()
            .Create();

        fixture.WireMock.Given(Request.Create().UsingPost()
            .WithPath("/users"))
            .RespondWith(Response.Create()
            .WithStatusCode(HttpStatusCode.Conflict));

        /* act: create the user through the client */
        var client = new UsersClient(fixture.HttpClient);
        var result = await client.CreateUserAsync(request, TestContext.Current.CancellationToken);

        /* assert: validate the returned failure */
        Assert.True(result.IsFailure);
        Assert.Equal(AuthorizationErrors.UserAlreadyExists, result.Error);
    }

    [Fact(DisplayName = "[integration] - when DeleteUserAsync is called with an existing user id, a successful result is returned")]
    public async Task When_DeleteUserAsync_IsCalled_WithExistingUserId_Then_SuccessfulResultIsReturned()
    {
        /* arrange: configures wiremock stub simulating keycloak user deletion */
        var userId = _fixture.Create<string>();

        fixture.WireMock.Given(Request.Create().UsingDelete()
            .WithPath($"/users/{userId}"))
            .RespondWith(Response.Create()
            .WithStatusCode(HttpStatusCode.NoContent));

        /* act: delete the user through the client */
        var client = new UsersClient(fixture.HttpClient);
        var result = await client.DeleteUserAsync(userId, TestContext.Current.CancellationToken);

        /* assert: validate the deletion succeeded */
        Assert.True(result.IsSuccess);
    }

    [Fact(DisplayName = "[integration] - when DeleteUserAsync is called with a non-existing user id, a failure result is returned")]
    public async Task When_DeleteUserAsync_IsCalled_WithNonExistingUserId_Then_FailureResultIsReturned()
    {
        /* arrange: configures wiremock stub simulating a missing keycloak user */
        var userId = _fixture.Create<string>();

        fixture.WireMock.Given(Request.Create().UsingDelete()
            .WithPath($"/users/{userId}"))
            .RespondWith(Response.Create()
            .WithStatusCode(HttpStatusCode.NotFound));

        /* act: delete the user through the client */
        var client = new UsersClient(fixture.HttpClient);
        var result = await client.DeleteUserAsync(userId, TestContext.Current.CancellationToken);

        /* assert: validate the returned failure */
        Assert.True(result.IsFailure);
        Assert.Equal(AuthorizationErrors.UserDoesNotExist, result.Error);
    }
}
