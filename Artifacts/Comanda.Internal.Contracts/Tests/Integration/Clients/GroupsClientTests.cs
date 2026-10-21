namespace Comanda.Internal.Contracts.Tests.Integration.Clients;

public sealed class GroupsClientTests(WireMockFixture fixture) : IClassFixture<WireMockFixture>
{
    private readonly Fixture _fixture = new();
    private readonly JsonSerializerOptions _serializer = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
    };

    [Fact(DisplayName = "[integration] - when GetGroupsAsync is called and keycloak responds with groups, a successful result with the paginated groups is returned")]
    public async Task When_GetGroupsAsync_IsCalled_AndKeycloakRespondsWithGroups_Then_SuccessfulResultIsReturned()
    {
        /* arrange: configures wiremock stub simulating the keycloak groups listing endpoint */
        var name = _fixture.Create<string>();
        var parameters = new GroupFiltersParameters().WithName(name);

        var groups = _fixture.CreateMany<GroupScheme>(2).ToList();

        fixture.WireMock.Given(Request.Create().UsingGet()
            .WithPath("/groups")
            .WithParam("search", name))
            .RespondWith(Response.Create()
            .WithStatusCode(HttpStatusCode.OK)
            .WithHeader(HeaderNames.ContentType, MediaTypeNames.Application.Json)
            .WithBody(JsonSerializer.Serialize(groups, _serializer)));

        /* act: request groups through the client */
        var client = new GroupsClient(fixture.HttpClient);
        var result = await client.GetGroupsAsync(parameters, TestContext.Current.CancellationToken);

        /* assert: validate the manually assembled pagination */
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.Equal(groups.Count, result.Data.Items.Count);
    }

    [Fact(DisplayName = "[integration] - when GetGroupsAsync is called and keycloak responds with an error, a failure result with the unknown error is returned")]
    public async Task When_GetGroupsAsync_IsCalled_AndKeycloakRespondsWithError_Then_FailureResultIsReturned()
    {
        /* arrange: configures wiremock stub simulating a keycloak failure */
        var name = _fixture.Create<string>();
        var parameters = new GroupFiltersParameters().WithName(name);

        fixture.WireMock.Given(Request.Create().UsingGet()
            .WithPath("/groups")
            .WithParam("search", name))
            .RespondWith(Response.Create()
            .WithStatusCode(HttpStatusCode.InternalServerError));

        /* act: request groups through the client */
        var client = new GroupsClient(fixture.HttpClient);
        var result = await client.GetGroupsAsync(parameters, TestContext.Current.CancellationToken);

        /* assert: validate the returned failure */
        Assert.True(result.IsFailure);
        Assert.Equal(Error.Unknown, result.Error);
    }

    [Fact(DisplayName = "[integration] - when CreateGroupAsync is called with a valid request, a successful result with the created group is returned")]
    public async Task When_CreateGroupAsync_IsCalled_WithValidRequest_Then_SuccessfulResultIsReturned()
    {
        /* arrange: builds a group creation request and the group keycloak will respond with */
        var request = _fixture.Build<GroupCreationScheme>()
            .Create();

        var groupId = Guid.NewGuid().ToString("N");
        var group = _fixture.Build<GroupScheme>()
            .With(scheme => scheme.Id, groupId)
            .With(scheme => scheme.Name, request.Name)
            .Create();

        /* arrange: configures wiremock stubs simulating keycloak group creation followed by the location lookup */
        fixture.WireMock.Given(Request.Create().UsingPost()
            .WithPath("/groups"))
            .RespondWith(Response.Create()
            .WithStatusCode(HttpStatusCode.Created)
            .WithHeader("Location", $"{fixture.WireMock.Url}/groups/{groupId}"));

        fixture.WireMock.Given(Request.Create().UsingGet()
            .WithPath($"/groups/{groupId}"))
            .RespondWith(Response.Create()
            .WithStatusCode(HttpStatusCode.OK)
            .WithHeader(HeaderNames.ContentType, MediaTypeNames.Application.Json)
            .WithBody(JsonSerializer.Serialize(group, _serializer)));

        /* act: create the group through the client */
        var client = new GroupsClient(fixture.HttpClient);
        var result = await client.CreateGroupAsync(request, TestContext.Current.CancellationToken);

        /* assert: validate the group was fetched back through the location header */
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.Equal(groupId, result.Data.Id);
        Assert.Equal(request.Name, result.Data.Name);
    }

    [Fact(DisplayName = "[integration] - when CreateGroupAsync is called and keycloak rejects the request, a failure result is returned")]
    public async Task When_CreateGroupAsync_IsCalled_AndKeycloakRejectsRequest_Then_FailureResultIsReturned()
    {
        /* arrange: builds a group creation request that will be rejected */
        var request = _fixture.Build<GroupCreationScheme>()
            .Create();

        fixture.WireMock.Given(Request.Create().UsingPost()
            .WithPath("/groups"))
            .RespondWith(Response.Create()
            .WithStatusCode(HttpStatusCode.Conflict));

        /* act: create the group through the client */
        var client = new GroupsClient(fixture.HttpClient);
        var result = await client.CreateGroupAsync(request, TestContext.Current.CancellationToken);

        /* assert: validate the returned failure */
        Assert.True(result.IsFailure);
        Assert.Equal(AuthorizationErrors.GroupAlreadyExists, result.Error);
    }

    [Fact(DisplayName = "[integration] - when GetGroupMembersAsync is called with an existing group, a successful result with the paginated members is returned")]
    public async Task When_GetGroupMembersAsync_IsCalled_WithExistingGroup_Then_SuccessfulResultIsReturned()
    {
        /* arrange: configures wiremock stub simulating the keycloak group members endpoint */
        var groupId = _fixture.Create<string>();
        var parameters = new GroupMembersFiltersParameters().WithGroupId(groupId);

        var members = _fixture.CreateMany<UserScheme>(2).ToList();

        fixture.WireMock.Given(Request.Create().UsingGet()
            .WithPath($"/groups/{groupId}/members"))
            .RespondWith(Response.Create()
            .WithStatusCode(HttpStatusCode.OK)
            .WithHeader(HeaderNames.ContentType, MediaTypeNames.Application.Json)
            .WithBody(JsonSerializer.Serialize(members, _serializer)));

        /* act: request group members through the client */
        var client = new GroupsClient(fixture.HttpClient);
        var result = await client.GetGroupMembersAsync(parameters, TestContext.Current.CancellationToken);

        /* assert: validate the manually assembled pagination */
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.Equal(members.Count, result.Data.Items.Count);
    }

    [Fact(DisplayName = "[integration] - when AssignUserGroupAsync is called with an existing group, a successful result is returned")]
    public async Task When_AssignUserGroupAsync_IsCalled_WithExistingGroup_Then_SuccessfulResultIsReturned()
    {
        /* arrange: configures wiremock stub simulating keycloak group membership assignment */
        var membership = _fixture.Create<GroupMembershipScheme>();

        fixture.WireMock.Given(Request.Create().UsingPut()
            .WithPath($"/users/{membership.UserId}/groups/{membership.GroupId}"))
            .RespondWith(Response.Create()
            .WithStatusCode(HttpStatusCode.NoContent));

        /* act: assign the user to the group through the client */
        var client = new GroupsClient(fixture.HttpClient);
        var result = await client.AssignUserGroupAsync(membership, TestContext.Current.CancellationToken);

        /* assert: validate the assignment succeeded */
        Assert.True(result.IsSuccess);
    }

    [Fact(DisplayName = "[integration] - when RemoveUserGroupAsync is called with a non-existing group, a failure result is returned")]
    public async Task When_RemoveUserGroupAsync_IsCalled_WithNonExistingGroup_Then_FailureResultIsReturned()
    {
        /* arrange: configures wiremock stub simulating a missing keycloak group */
        var membership = _fixture.Create<GroupMembershipScheme>();

        fixture.WireMock.Given(Request.Create().UsingDelete()
            .WithPath($"/users/{membership.UserId}/groups/{membership.GroupId}"))
            .RespondWith(Response.Create()
            .WithStatusCode(HttpStatusCode.NotFound));

        /* act: remove the user from the group through the client */
        var client = new GroupsClient(fixture.HttpClient);
        var result = await client.RemoveUserGroupAsync(membership, TestContext.Current.CancellationToken);

        /* assert: validate the returned failure */
        Assert.True(result.IsFailure);
        Assert.Equal(AuthorizationErrors.GroupDoesNotExist, result.Error);
    }
}
