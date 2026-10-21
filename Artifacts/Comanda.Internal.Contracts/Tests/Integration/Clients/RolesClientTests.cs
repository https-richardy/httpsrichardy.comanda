namespace Comanda.Internal.Contracts.Tests.Integration.Clients;

public sealed class RolesClientTests(WireMockFixture fixture) : IClassFixture<WireMockFixture>
{
    private readonly Fixture _fixture = new();
    private readonly JsonSerializerOptions _serializer = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
    };

    [Fact(DisplayName = "[integration] - when GetRolesAsync is called and keycloak responds with roles, a successful result with the roles bound to the requested client is returned")]
    public async Task When_GetRolesAsync_IsCalled_AndKeycloakRespondsWithRoles_Then_SuccessfulResultIsReturned()
    {
        /* arrange: configures wiremock stub simulating the keycloak client roles listing endpoint */
        var clientIdentifier = _fixture.Create<string>();
        var parameters = new RoleFiltersParameters().WithClientIdentifier(clientIdentifier);

        var roles = _fixture.CreateMany<RoleScheme>(2).ToList();

        fixture.WireMock.Given(Request.Create().UsingGet()
            .WithPath($"/clients/{clientIdentifier}/roles"))
            .RespondWith(Response.Create()
            .WithStatusCode(HttpStatusCode.OK)
            .WithHeader(HeaderNames.ContentType, MediaTypeNames.Application.Json)
            .WithBody(JsonSerializer.Serialize(roles, _serializer)));

        /* act: request roles through the client */
        var client = new RolesClient(fixture.HttpClient);
        var result = await client.GetRolesAsync(parameters, TestContext.Current.CancellationToken);

        /* assert: validate every returned role is bound to the requested client, regardless of the raw response */
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.Equal(roles.Count, result.Data.Items.Count);
        Assert.All(result.Data.Items, role => Assert.Equal(clientIdentifier, role.ClientIdentifier));
    }

    [Fact(DisplayName = "[integration] - when GetRolesAsync is called and keycloak responds with an error, a failure result is returned")]
    public async Task When_GetRolesAsync_IsCalled_AndKeycloakRespondsWithError_Then_FailureResultIsReturned()
    {
        /* arrange: configures wiremock stub simulating a missing keycloak client */
        var clientIdentifier = _fixture.Create<string>();
        var parameters = new RoleFiltersParameters().WithClientIdentifier(clientIdentifier);

        fixture.WireMock.Given(Request.Create().UsingGet()
            .WithPath($"/clients/{clientIdentifier}/roles"))
            .RespondWith(Response.Create()
            .WithStatusCode(HttpStatusCode.NotFound));

        /* act: request roles through the client */
        var client = new RolesClient(fixture.HttpClient);
        var result = await client.GetRolesAsync(parameters, TestContext.Current.CancellationToken);

        /* assert: validate the returned failure */
        Assert.True(result.IsFailure);
        Assert.Equal(AuthorizationErrors.ClientDoesNotExist, result.Error);
    }

    [Fact(DisplayName = "[integration] - when GetRoleAsync is called with an existing role id, a successful result with the mapped role is returned")]
    public async Task When_GetRoleAsync_IsCalled_WithExistingRoleId_Then_SuccessfulResultIsReturned()
    {
        /* arrange: configures wiremock stub simulating the keycloak role-by-id representation */
        var representation = _fixture.Create<RoleRepresentation>();

        fixture.WireMock.Given(Request.Create().UsingGet()
            .WithPath($"/roles-by-id/{representation.Id}"))
            .RespondWith(Response.Create()
            .WithStatusCode(HttpStatusCode.OK)
            .WithHeader(HeaderNames.ContentType, MediaTypeNames.Application.Json)
            .WithBody(JsonSerializer.Serialize(representation, _serializer)));

        /* act: request the role through the client */
        var client = new RolesClient(fixture.HttpClient);
        var result = await client.GetRoleAsync(representation.Id, TestContext.Current.CancellationToken);

        /* assert: validate the keycloak representation's containerId is mapped to the scheme's clientIdentifier */
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.Equal(representation.Id, result.Data.Id);
        Assert.Equal(representation.ContainerId, result.Data.ClientIdentifier);
    }

    [Fact(DisplayName = "[integration] - when GetRoleAsync is called with a non-existing role id, a failure result is returned")]
    public async Task When_GetRoleAsync_IsCalled_WithNonExistingRoleId_Then_FailureResultIsReturned()
    {
        /* arrange: configures wiremock stub simulating a missing keycloak role */
        var roleId = _fixture.Create<string>();

        fixture.WireMock.Given(Request.Create().UsingGet()
            .WithPath($"/roles-by-id/{roleId}"))
            .RespondWith(Response.Create()
            .WithStatusCode(HttpStatusCode.NotFound));

        /* act: request the role through the client */
        var client = new RolesClient(fixture.HttpClient);
        var result = await client.GetRoleAsync(roleId, TestContext.Current.CancellationToken);

        /* assert: validate the returned failure */
        Assert.True(result.IsFailure);
        Assert.Equal(AuthorizationErrors.RoleDoesNotExist, result.Error);
    }

    [Fact(DisplayName = "[integration] - when CreateRoleAsync is called with a valid request, a successful result with the created role is returned")]
    public async Task When_CreateRoleAsync_IsCalled_WithValidRequest_Then_SuccessfulResultIsReturned()
    {
        /* arrange: builds a role creation request and the role keycloak will respond with */
        var request = _fixture.Build<RoleCreationScheme>()
            .Create();

        var role = _fixture.Build<RoleScheme>()
            .With(scheme => scheme.Name, request.Name)
            .With(scheme => scheme.Description, request.Description)
            .Create();

        /* arrange: configures wiremock stubs simulating keycloak role creation followed by the location lookup */
        fixture.WireMock.Given(Request.Create().UsingPost()
            .WithPath($"/clients/{request.ClientIdentifier}/roles"))
            .RespondWith(Response.Create()
            .WithStatusCode(HttpStatusCode.Created)
            .WithHeader("Location", $"{fixture.WireMock.Url}/clients/{request.ClientIdentifier}/roles/{request.Name}"));

        fixture.WireMock.Given(Request.Create().UsingGet()
            .WithPath($"/clients/{request.ClientIdentifier}/roles/{request.Name}"))
            .RespondWith(Response.Create()
            .WithStatusCode(HttpStatusCode.OK)
            .WithHeader(HeaderNames.ContentType, MediaTypeNames.Application.Json)
            .WithBody(JsonSerializer.Serialize(role, _serializer)));

        /* act: create the role through the client */
        var client = new RolesClient(fixture.HttpClient);
        var result = await client.CreateRoleAsync(request, TestContext.Current.CancellationToken);

        /* assert: validate the role was fetched back through the location header and bound to the requested client */
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.Equal(request.Name, result.Data.Name);
        Assert.Equal(request.ClientIdentifier, result.Data.ClientIdentifier);
    }

    [Fact(DisplayName = "[integration] - when CreateRoleAsync is called and keycloak rejects the request, a failure result is returned")]
    public async Task When_CreateRoleAsync_IsCalled_AndKeycloakRejectsRequest_Then_FailureResultIsReturned()
    {
        /* arrange: builds a role creation request that will be rejected */
        var request = _fixture.Build<RoleCreationScheme>()
            .Create();

        fixture.WireMock.Given(Request.Create().UsingPost()
            .WithPath($"/clients/{request.ClientIdentifier}/roles"))
            .RespondWith(Response.Create()
            .WithStatusCode(HttpStatusCode.Conflict));

        /* act: create the role through the client */
        var client = new RolesClient(fixture.HttpClient);
        var result = await client.CreateRoleAsync(request, TestContext.Current.CancellationToken);

        /* assert: validate the returned failure */
        Assert.True(result.IsFailure);
        Assert.Equal(AuthorizationErrors.RoleAlreadyExists, result.Error);
    }

    [Fact(DisplayName = "[integration] - when AssignUserRoleAsync is called with an existing role, a successful result is returned")]
    public async Task When_AssignUserRoleAsync_IsCalled_WithExistingRole_Then_SuccessfulResultIsReturned()
    {
        /* arrange: configures wiremock stubs simulating the role lookup followed by the user role-mapping assignment */
        var representation = _fixture.Create<RoleRepresentation>();
        var subjectId = _fixture.Create<string>();

        fixture.WireMock.Given(Request.Create().UsingGet()
            .WithPath($"/roles-by-id/{representation.Id}"))
            .RespondWith(Response.Create()
            .WithStatusCode(HttpStatusCode.OK)
            .WithHeader(HeaderNames.ContentType, MediaTypeNames.Application.Json)
            .WithBody(JsonSerializer.Serialize(representation, _serializer)));

        fixture.WireMock.Given(Request.Create().UsingPost()
            .WithPath($"/users/{subjectId}/role-mappings/clients/{representation.ContainerId}"))
            .RespondWith(Response.Create()
            .WithStatusCode(HttpStatusCode.NoContent));

        /* act: assign the role through the client */
        var client = new RolesClient(fixture.HttpClient);
        var assignment = new RoleAssignmentScheme()
            .WithRoleId(representation.Id)
            .WithSubjectId(subjectId);

        var result = await client.AssignUserRoleAsync(assignment, TestContext.Current.CancellationToken);

        /* assert: validate the assignment succeeded */
        Assert.True(result.IsSuccess);
    }

    [Fact(DisplayName = "[integration] - when AssignUserRoleAsync is called with a non-existing role, a failure result is returned and the assignment is never attempted")]
    public async Task When_AssignUserRoleAsync_IsCalled_WithNonExistingRole_Then_FailureResultIsReturnedAndAssignmentIsNeverAttempted()
    {
        /* arrange: configures wiremock stub simulating a missing keycloak role */
        var roleId = _fixture.Create<string>();
        var subjectId = _fixture.Create<string>();

        fixture.WireMock.Given(Request.Create().UsingGet()
            .WithPath($"/roles-by-id/{roleId}"))
            .RespondWith(Response.Create()
            .WithStatusCode(HttpStatusCode.NotFound));

        /* act: assign the role through the client */
        var client = new RolesClient(fixture.HttpClient);
        var assignment = new RoleAssignmentScheme()
            .WithRoleId(roleId)
            .WithSubjectId(subjectId);

        var result = await client.AssignUserRoleAsync(assignment, TestContext.Current.CancellationToken);

        /* assert: validate the returned failure */
        Assert.True(result.IsFailure);
        Assert.Equal(AuthorizationErrors.RoleDoesNotExist, result.Error);

        /* assert: verify the assignment request was short-circuited and never sent */
        var requests = fixture.WireMock.FindLogEntries(
        [
            Request.Create().UsingPost().WithPath("/users/*/role-mappings/clients/*")
        ]);

        Assert.Empty(requests);
    }
}
