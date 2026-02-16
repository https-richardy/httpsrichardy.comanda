namespace Comanda.Profiles.TestSuite.Endpoints;

public sealed class OwnersEndpointTests(IntegrationEnvironmentFixture factory) :
    IClassFixture<IntegrationEnvironmentFixture>,
    IAsyncLifetime
{
    private readonly Fixture _fixture = new();
    private readonly JsonSerializerOptions _serializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    [Fact(DisplayName = "[e2e] - when GET /api/v1/owners is called and there are owners, 200 OK is returned with the owners")]
    public async Task When_GetOwners_IsCalled_AndThereAreOwners_Then_200OkIsReturnedWithTheOwners()
    {
        /* arrange: resolve http client and collection instances from integration environment */
        var httpClient = factory.HttpClient;
        var collection = factory.Services.GetRequiredService<IOwnerCollection>();

        /* arrange: create 3 non-deleted owners to be inserted in the database */
        var owners = _fixture.Build<Owner>()
            .With(owner => owner.IsDeleted, false)
            .CreateMany(3)
            .ToList();

        await collection.InsertManyAsync(owners, cancellation: TestContext.Current.CancellationToken);

        /* act: send GET request to the owners endpoint */
        var response = await httpClient.GetAsync("/api/v1/owners", TestContext.Current.CancellationToken);
        var content = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);

        /* assert: verify http response status and content */
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.False(string.IsNullOrWhiteSpace(content));

        /* assert: deserialize response and validate data structure */
        var result = JsonSerializer.Deserialize<IEnumerable<OwnerScheme>>(content, _serializerOptions);

        /* assert: ensure response contains owners */
        Assert.NotNull(result);
        Assert.NotEmpty(result);

        /* assert: check if the number of returned owners matches the inserted ones */
        Assert.Equal(owners.Count, result.Count());
    }

    [Fact(DisplayName = "[e2e] - when POST /api/v1/owners is called with valid data, 201 Created is returned with the created owner")]
    public async Task When_PostOwners_IsCalled_WithValidData_Then_201CreatedIsReturnedWithTheCreatedOwner()
    {
        /* arrange: resolves http client from integration environment */
        var httpClient = factory.HttpClient;

        /* arrange: create a valid owner payload according to validation rules */
        var request = _fixture.Build<OwnerCreationScheme>()
            .With(owner => owner.FirstName, "Richard")
            .With(owner => owner.LastName, "Garcia")
            .With(owner => owner.Email, "richard.garcia@example.com")
            .With(owner => owner.PhoneNumber, "11999999999")
            .With(owner => owner.Username, "richardgarcia")
            .With(owner => owner.UserId, Identifier.Generate<Owner>())
            .Create();

        /* act: send POST request to create the new owner */
        var response = await httpClient.PostAsJsonAsync("/api/v1/owners", request, TestContext.Current.CancellationToken);
        var content = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);

        /* assert: verify http status code and content body */
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.False(string.IsNullOrWhiteSpace(content));

        /* assert: deserialize response to validate returned structure */
        var result = JsonSerializer.Deserialize<OwnerScheme>(content, _serializerOptions);

        /* assert: validate returned data matches input fields */
        Assert.NotNull(result);

        Assert.Equal(request.FirstName, result.FirstName);
        Assert.Equal(request.LastName, result.LastName);

        Assert.Equal(request.Email, result.Email);
        Assert.Equal(request.PhoneNumber, result.PhoneNumber);

        Assert.Equal(request.Username, result.Username);
        Assert.Equal(request.UserId, result.UserId);
    }

    [Fact(DisplayName = "[e2e] - when POST /api/v1/owners is called with an existing email, 409 Conflict is returned [#COMANDA-ERROR-76A71]")]
    public async Task When_PostOwners_IsCalled_WithExistingEmail_Then_409ConflictIsReturned()
    {
        /* arrange: resolve http client and owner collection */
        var httpClient = factory.HttpClient;
        var collection = factory.Services.GetRequiredService<IOwnerCollection>();

        /* arrange: insert an existing owner with specific email */
        var existingOwner = _fixture.Build<Owner>()
            .With(owner => owner.FirstName, "Richard")
            .With(owner => owner.LastName, "Garcia")
            .With(owner => owner.Contact, new Contact("richard.garcia@example.com", "11988888888"))
            .With(owner => owner.IsDeleted, false)
            .Create();

        await collection.InsertAsync(existingOwner, cancellation: TestContext.Current.CancellationToken);

        /* arrange: create a new owner payload with the same email */
        var request = _fixture.Build<OwnerCreationScheme>()
            .With(owner => owner.FirstName, "Richard")
            .With(owner => owner.LastName, "Garcia")
            .With(owner => owner.Email, "richard.garcia@example.com")
            .With(owner => owner.PhoneNumber, "11999999999")
            .With(owner => owner.Username, "richardgarcia2")
            .With(owner => owner.UserId, Identifier.Generate<Owner>())
            .Create();

        /* act: send POST request to create the duplicate owner */
        var response = await httpClient.PostAsJsonAsync("/api/v1/owners", request, TestContext.Current.CancellationToken);
        var content = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);

        /* assert: verify http status and content body */
        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        Assert.False(string.IsNullOrWhiteSpace(content));

        /* assert: deserialize and ensure error matches #COMANDA-ERROR-76A71 */
        var error = JsonSerializer.Deserialize<Error>(content, _serializerOptions);

        Assert.NotNull(error);
        Assert.Equal(ProfileErrors.ProfileAlreadyExists, error);
    }

    [Fact(DisplayName = "[e2e] - when PUT /api/v1/owners/{id} is called with valid data, 200 OK is returned with the updated owner")]
    public async Task When_PutOwners_IsCalled_WithValidData_Then_200OkIsReturnedWithTheUpdatedOwner()
    {
        /* arrange: resolve http client and collection from integration environment */
        var httpClient = factory.HttpClient;
        var collection = factory.Services.GetRequiredService<IOwnerCollection>();

        /* arrange: insert an existing owner in the database */
        var existingOwner = _fixture.Build<Owner>()
            .With(owner => owner.FirstName, "Richard")
            .With(owner => owner.LastName, "Garcia")
            .With(owner => owner.Contact, new Contact("richard.garcia@example.com", "11999999999"))
            .With(owner => owner.IsDeleted, false)
            .Create();

        await collection.InsertAsync(existingOwner, cancellation: TestContext.Current.CancellationToken);

        /* arrange: create a valid edit payload with new data */
        var request = _fixture.Build<EditOwnerScheme>()
            .With(owner => owner.FirstName, "Ricardo")
            .With(owner => owner.LastName, "Gonçalves")
            .With(owner => owner.Email, "ricardo.goncalves@example.com")
            .With(owner => owner.PhoneNumber, "11988887777")
            .Create();

        /* act: send PUT request to update the existing owner */
        var response = await httpClient.PutAsJsonAsync($"/api/v1/owners/{existingOwner.Id}", request, TestContext.Current.CancellationToken);
        var content = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);

        /* assert: verify http status and content body */
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.False(string.IsNullOrWhiteSpace(content));

        /* assert: deserialize response to validate returned structure */
        var result = JsonSerializer.Deserialize<OwnerScheme>(content, _serializerOptions);

        Assert.NotNull(result);

        /* assert: ensure returned owner matches the updated values */
        Assert.Equal(existingOwner.Id, result.Identifier);
        Assert.Equal(request.FirstName, result.FirstName);
        Assert.Equal(request.LastName, result.LastName);
        Assert.Equal(request.Email, result.Email);
        Assert.Equal(request.PhoneNumber, result.PhoneNumber);
    }

    [Fact(DisplayName = "[e2e] - when PUT /api/v1/owners/{id} is called with a non-existent owner, 404 NotFound is returned [#COMANDA-ERROR-0831D]")]
    public async Task When_PutOwners_IsCalled_WithNonExistentOwner_Then_404NotFoundIsReturned()
    {
        /* arrange: resolve http client from integration environment */
        var httpClient = factory.HttpClient;

        /* arrange: define a non-existent owner identifier */
        var nonExistentOwnerId = Identifier.Generate<Owner>();

        /* arrange: create a valid edit payload */
        var request = _fixture.Build<EditOwnerScheme>()
            .With(owner => owner.FirstName, "Ricardo")
            .With(owner => owner.LastName, "Gonçalves")
            .With(owner => owner.Email, "ricardo.goncalves@example.com")
            .With(owner => owner.PhoneNumber, "11988887777")
            .Create();

        /* act: send PUT request for a non-existent owner */
        var response = await httpClient.PutAsJsonAsync($"/api/v1/owners/{nonExistentOwnerId}", request, TestContext.Current.CancellationToken);
        var content = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);

        /* assert: verify http status and content body */
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.False(string.IsNullOrWhiteSpace(content));

        /* assert: deserialize and ensure error matches #COMANDA-ERROR-0831D */
        var error = JsonSerializer.Deserialize<Error>(content, _serializerOptions);

        Assert.NotNull(error);
        Assert.Equal(OwnerErrors.OwnerDoesNotExist, error);
    }

    [Fact(DisplayName = "[e2e] - when DELETE /api/v1/owners/{id} is called with an existing owner, 204 NoContent is returned")]
    public async Task When_DeleteOwners_IsCalled_WithExistingOwner_Then_204NoContentIsReturned()
    {
        /* arrange: resolve http client and collection from integration environment */
        var httpClient = factory.HttpClient;
        var collection = factory.Services.GetRequiredService<IOwnerCollection>();

        /* arrange: insert an existing owner in the database */
        var existingOwner = _fixture.Build<Owner>()
            .With(owner => owner.FirstName, "Richard")
            .With(owner => owner.LastName, "Garcia")
            .With(owner => owner.Contact, new Contact("richard.garcia@example.com", "11999999999"))
            .With(owner => owner.IsDeleted, false)
            .Create();

        await collection.InsertAsync(existingOwner, cancellation: TestContext.Current.CancellationToken);

        /* act: send DELETE request for the existing owner */
        var response = await httpClient.DeleteAsync($"/api/v1/owners/{existingOwner.Id}", TestContext.Current.CancellationToken);

        /* assert: verify http response status */
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        /* assert: verify that the owner is now marked as deleted */
        var filters = OwnerFilters.WithSpecifications()
            .WithIdentifier(existingOwner.Id)
            .WithIsDeleted(true)
            .Build();

        var deletedOwners = await collection.GetOwnersAsync(filters, TestContext.Current.CancellationToken);
        var deletedOwner = deletedOwners.FirstOrDefault();

        /* assert: ensure owner exists and is marked as deleted */
        Assert.NotNull(deletedOwner);
        Assert.True(deletedOwner.IsDeleted);
    }

    [Fact(DisplayName = "[e2e] - when DELETE /api/v1/owners/{id} is called with a non-existing owner, 404 NotFound is returned [#COMANDA-ERROR-0831D]")]
    public async Task When_DeleteOwners_IsCalled_WithNonExistingOwner_Then_404NotFoundIsReturned()
    {
        /* arrange: resolve http client from integration environment */
        var httpClient = factory.HttpClient;

        /* arrange: create a random non-existing owner id */
        var nonExistingOwnerId = Identifier.Generate<Owner>();

        /* act: send DELETE request for a non-existing owner */
        var response = await httpClient.DeleteAsync($"/api/v1/owners/{nonExistingOwnerId}", TestContext.Current.CancellationToken);
        var content = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);

        /* assert: verify http response status and body */
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.False(string.IsNullOrWhiteSpace(content));

        /* assert: deserialize and ensure error matches #COMANDA-ERROR-0831D */
        var error = JsonSerializer.Deserialize<Error>(content, _serializerOptions);

        Assert.NotNull(error);
        Assert.Equal(OwnerErrors.OwnerDoesNotExist, error);
    }

    public ValueTask InitializeAsync() => factory.InitializeAsync();
    public ValueTask DisposeAsync() => factory.DisposeAsync();
}
