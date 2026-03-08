namespace Comanda.Stores.TestSuite.Endpoints;

public sealed class EstablishmentsEndpointTests(IntegrationEnvironmentFixture factory) :
    IClassFixture<IntegrationEnvironmentFixture>,
    IAsyncLifetime
{
    private readonly Fixture _fixture = new();
    private readonly JsonSerializerOptions _serializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    [Fact(DisplayName = "[e2e] - when GET /api/v1/establishments is called and there are establishments, 200 OK is returned with the establishments")]
    public async Task When_GetEstablishments_IsCalled_AndThereAreEstablishments_Then_200OkIsReturnedWithTheEstablishments()
    {
        /* arrange: resolves http client and collection instances from integration environment */
        var httpClient = factory.HttpClient;
        var collection = factory.Services.GetRequiredService<IEstablishmentCollection>();

        /* arrange: create 3 non-deleted establishments to be inserted in the database */
        var establishments = _fixture.Build<Establishment>()
            .With(establishment => establishment.IsDeleted, false)
            .CreateMany(3)
            .ToList();

        await collection.InsertManyAsync(establishments, cancellation: TestContext.Current.CancellationToken);

        /* act: send GET request to the establishments endpoint */
        var response = await httpClient.GetAsync("/api/v1/establishments", TestContext.Current.CancellationToken);
        var content = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);

        /* assert: verify http response status and content */
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.False(string.IsNullOrWhiteSpace(content));

        /* assert: deserialize response and validate data structure */
        var result = JsonSerializer.Deserialize<IEnumerable<EstablishmentScheme>>(content, _serializerOptions);

        /* assert: ensure response contains establishments */
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(establishments.Count, result.Count());
    }

    [Fact(DisplayName = "[e2e] - when POST /api/v1/establishments is called with a valid payload, 200 OK is returned")]
    public async Task When_CreateEstablishment_IsCalled_WithValidPayload_Then_200OkIsReturned()
    {
        var httpClient = factory.HttpClient;
        var request = _fixture.Build<EstablishmentCreationScheme>()
            .With(item => item.Title, "Estabelecimento Fixture")
            .With(item => item.Description, "Descrição criada com fixture")
            .With(item => item.Owner, new User(Identifier.Generate<User>(), "john.doe@email.com"))
            .Create();

        var response = await httpClient.PostAsJsonAsync("/api/v1/establishments", request, _serializerOptions, TestContext.Current.CancellationToken);
        var content = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.False(string.IsNullOrWhiteSpace(content));

        var result = JsonSerializer.Deserialize<EstablishmentScheme>(content, _serializerOptions);

        Assert.NotNull(result);
        Assert.Equal(request.Title, result.Title);
        Assert.Equal(request.Description, result.Description);
    }

    [Fact(DisplayName = "[e2e] - when POST /api/v1/establishments is called and owner already has an establishment, 409 Conflict is returned")]
    public async Task When_CreateEstablishment_IsCalled_AndOwnerAlreadyHasEstablishment_Then_409ConflictIsReturned()
    {
        var httpClient = factory.HttpClient;
        var request = _fixture.Build<EstablishmentCreationScheme>()
            .With(item => item.Title, "Estabelecimento Fixture")
            .With(item => item.Description, "Descrição criada com fixture")
            .With(item => item.Owner, new User(Identifier.Generate<User>(), "john.doe@email.com"))
            .Create();

        var firstResponse = await httpClient.PostAsJsonAsync("/api/v1/establishments", request, _serializerOptions, TestContext.Current.CancellationToken);
        var secondResponse = await httpClient.PostAsJsonAsync("/api/v1/establishments", request, _serializerOptions, TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.OK, firstResponse.StatusCode);
        Assert.Equal(HttpStatusCode.Conflict, secondResponse.StatusCode);
    }

    [Fact(DisplayName = "[e2e] - when PUT /api/v1/establishments/{id} is called and establishment exists, 200 OK is returned")]
    public async Task When_UpdateEstablishment_IsCalled_AndEstablishmentExists_Then_200OkIsReturned()
    {
        var httpClient = factory.HttpClient;
        var collection = factory.Services.GetRequiredService<IEstablishmentCollection>();

        var establishment = _fixture.Build<Establishment>()
            .With(item => item.IsDeleted, false)
            .Create();

        await collection.InsertAsync(establishment, cancellation: TestContext.Current.CancellationToken);

        var request = _fixture.Build<EstablishmentEditionScheme>()
            .With(item => item.Title, "Estabelecimento Atualizado")
            .With(item => item.Description, "Descrição atualizada com fixture")
            .With(item => item.Branding, new Branding("#1D4ED8", "#22C55E", "https://assets.example.com/logo.png"))
            .Create();

        var response = await httpClient.PutAsJsonAsync($"/api/v1/establishments/{establishment.Id}", request, _serializerOptions, TestContext.Current.CancellationToken);
        var content = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.False(string.IsNullOrWhiteSpace(content));

        var result = JsonSerializer.Deserialize<EstablishmentScheme>(content, _serializerOptions);

        Assert.NotNull(result);
        Assert.Equal(request.Title, result.Title);
        Assert.Equal(request.Description, result.Description);
    }

    [Fact(DisplayName = "[e2e] - when PUT /api/v1/establishments/{id} is called and establishment does not exist, 404 NotFound is returned")]
    public async Task When_UpdateEstablishment_IsCalled_AndEstablishmentDoesNotExist_Then_404NotFoundIsReturned()
    {
        var httpClient = factory.HttpClient;

        var request = _fixture.Build<EstablishmentEditionScheme>()
            .With(item => item.Title, "Estabelecimento Atualizado")
            .With(item => item.Description, "Descrição atualizada com fixture")
            .With(item => item.Branding, new Branding("#1D4ED8", "#22C55E", "https://assets.example.com/logo.png"))
            .Create();

        var response = await httpClient.PutAsJsonAsync("/api/v1/establishments/non-existent-id", request, _serializerOptions, TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact(DisplayName = "[e2e] - when DELETE /api/v1/establishments/{id} is called and establishment exists, 204 NoContent is returned")]
    public async Task When_DeleteEstablishment_IsCalled_AndEstablishmentExists_Then_204NoContentIsReturned()
    {
        var httpClient = factory.HttpClient;
        var collection = factory.Services.GetRequiredService<IEstablishmentCollection>();

        var establishment = _fixture.Build<Establishment>()
            .With(item => item.IsDeleted, false)
            .Create();

        await collection.InsertAsync(establishment, cancellation: TestContext.Current.CancellationToken);

        var response = await httpClient.DeleteAsync($"/api/v1/establishments/{establishment.Id}", TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        var filters = EstablishmentFilters.WithSpecifications()
            .WithIdentifier(establishment.Id)
            .Build();

        var deletedEstablishments = await collection.FilterEstablishmentsAsync(filters, TestContext.Current.CancellationToken);

        Assert.Empty(deletedEstablishments);
    }

    [Fact(DisplayName = "[e2e] - when DELETE /api/v1/establishments/{id} is called and establishment does not exist, 404 NotFound is returned")]
    public async Task When_DeleteEstablishment_IsCalled_AndEstablishmentDoesNotExist_Then_404NotFoundIsReturned()
    {
        var httpClient = factory.HttpClient;
        var response = await httpClient.DeleteAsync("/api/v1/establishments/non-existent-id", TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact(DisplayName = "[e2e] - when GET /api/v1/establishments/{id}/products is called and there are products, 200 OK is returned")]
    public async Task When_GetProducts_IsCalled_AndThereAreProducts_Then_200OkIsReturned()
    {
        var httpClient = factory.HttpClient;

        var establishmentCollection = factory.Services.GetRequiredService<IEstablishmentCollection>();
        var productCollection = factory.Services.GetRequiredService<IProductCollection>();

        var establishment = _fixture.Build<Establishment>()
            .With(item => item.IsDeleted, false)
            .Create();

        await establishmentCollection.InsertAsync(establishment, cancellation: TestContext.Current.CancellationToken);

        var products = _fixture.Build<Product>()
            .With(product => product.Metadata, new ProductMetadata(establishment.Id))
            .With(product => product.IsDeleted, false)
            .CreateMany(2)
            .ToList();

        await productCollection.InsertManyAsync(products, cancellation: TestContext.Current.CancellationToken);

        var response = await httpClient.GetAsync($"/api/v1/establishments/{establishment.Id}/products", TestContext.Current.CancellationToken);
        var content = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.False(string.IsNullOrWhiteSpace(content));

        var result = JsonSerializer.Deserialize<IEnumerable<ProductScheme>>(content, _serializerOptions);

        Assert.NotNull(result);
        Assert.NotEmpty(result);
    }

    [Fact(DisplayName = "[e2e] - when POST /api/v1/establishments/{id}/products is called with valid payload, 201 Created is returned")]
    public async Task When_CreateProduct_IsCalled_WithValidPayload_Then_201CreatedIsReturned()
    {
        var httpClient = factory.HttpClient;
        var establishmentCollection = factory.Services.GetRequiredService<IEstablishmentCollection>();

        var establishment = _fixture.Build<Establishment>()
            .With(item => item.IsDeleted, false)
            .Create();

        await establishmentCollection.InsertAsync(establishment, cancellation: TestContext.Current.CancellationToken);

        var request = _fixture.Build<ProductCreationScheme>()
            .With(item => item.Title, "Produto Fixture")
            .With(item => item.Description, "Descrição de produto fixture")
            .With(item => item.Price, 59.90m)
            .Create();

        var response = await httpClient.PostAsJsonAsync($"/api/v1/establishments/{establishment.Id}/products", request, _serializerOptions, TestContext.Current.CancellationToken);
        var content = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.False(string.IsNullOrWhiteSpace(content));

        var result = JsonSerializer.Deserialize<ProductScheme>(content, _serializerOptions);

        Assert.NotNull(result);
        Assert.Equal(request.Title, result.Title);
        Assert.Equal(request.Description, result.Description);
        Assert.Equal(request.Price, result.Price);
    }

    [Fact(DisplayName = "[e2e] - when PUT /api/v1/establishments/{id}/products/{productId} is called and data is valid, 200 OK is returned")]
    public async Task When_UpdateProduct_IsCalled_AndDataIsValid_Then_200OkIsReturned()
    {
        var httpClient = factory.HttpClient;

        var establishmentCollection = factory.Services.GetRequiredService<IEstablishmentCollection>();
        var productCollection = factory.Services.GetRequiredService<IProductCollection>();

        var establishment = _fixture.Build<Establishment>()
            .With(item => item.IsDeleted, false)
            .Create();

        await establishmentCollection.InsertAsync(establishment, cancellation: TestContext.Current.CancellationToken);

        var product = _fixture.Build<Product>()
            .With(item => item.Metadata, new ProductMetadata(establishment.Id))
            .With(item => item.IsDeleted, false)
            .Create();

        await productCollection.InsertAsync(product, cancellation: TestContext.Current.CancellationToken);

        var request = _fixture.Build<ProductEditionScheme>()
            .With(item => item.Title, "Produto Atualizado")
            .With(item => item.Description, "Descrição atualizada com fixture")
            .With(item => item.Price, 99.90m)
            .Create();

        var response = await httpClient.PutAsJsonAsync($"/api/v1/establishments/{establishment.Id}/products/{product.Id}", request, _serializerOptions, TestContext.Current.CancellationToken);
        var content = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.False(string.IsNullOrWhiteSpace(content));

        var result = JsonSerializer.Deserialize<ProductScheme>(content, _serializerOptions);

        Assert.NotNull(result);
        Assert.Equal(request.Title, result.Title);
        Assert.Equal(request.Description, result.Description);
        Assert.Equal(request.Price, result.Price);
    }

    [Fact(DisplayName = "[e2e] - when PUT /api/v1/establishments/{id}/products/{productId} is called and establishment does not exist, 404 NotFound is returned")]
    public async Task When_UpdateProduct_IsCalled_AndEstablishmentDoesNotExist_Then_404NotFoundIsReturned()
    {
        var httpClient = factory.HttpClient;
        var productCollection = factory.Services.GetRequiredService<IProductCollection>();

        var product = _fixture.Build<Product>()
            .With(item => item.Metadata, new ProductMetadata("another-establishment"))
            .With(item => item.IsDeleted, false)
            .Create();

        await productCollection.InsertAsync(product, cancellation: TestContext.Current.CancellationToken);

        var request = _fixture.Build<ProductEditionScheme>()
            .With(item => item.Title, "Produto Atualizado")
            .With(item => item.Description, "Descrição atualizada com fixture")
            .With(item => item.Price, 99.90m)
            .Create();

        var response = await httpClient.PutAsJsonAsync($"/api/v1/establishments/non-existent-id/products/{product.Id}", request, _serializerOptions, TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact(DisplayName = "[e2e] - when PUT /api/v1/establishments/{id}/products/{productId} is called and product does not exist, 404 NotFound is returned")]
    public async Task When_UpdateProduct_IsCalled_AndProductDoesNotExist_Then_404NotFoundIsReturned()
    {
        var httpClient = factory.HttpClient;
        var establishmentCollection = factory.Services.GetRequiredService<IEstablishmentCollection>();

        var establishment = _fixture.Build<Establishment>()
            .With(item => item.IsDeleted, false)
            .Create();

        await establishmentCollection.InsertAsync(establishment, cancellation: TestContext.Current.CancellationToken);

        var request = _fixture.Build<ProductEditionScheme>()
            .With(item => item.Title, "Produto Atualizado")
            .With(item => item.Description, "Descrição atualizada com fixture")
            .With(item => item.Price, 99.90m)
            .Create();

        var response = await httpClient.PutAsJsonAsync($"/api/v1/establishments/{establishment.Id}/products/non-existent-product", request, _serializerOptions, TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact(DisplayName = "[e2e] - when PUT /api/v1/establishments/{id}/products/{productId} is called and product belongs to another establishment, 422 UnprocessableEntity is returned")]
    public async Task When_UpdateProduct_IsCalled_AndProductBelongsToAnotherEstablishment_Then_422UnprocessableEntityIsReturned()
    {
        var httpClient = factory.HttpClient;

        var establishmentCollection = factory.Services.GetRequiredService<IEstablishmentCollection>();
        var productCollection = factory.Services.GetRequiredService<IProductCollection>();

        var establishment = _fixture.Build<Establishment>()
            .With(item => item.IsDeleted, false)
            .Create();

        var anotherEstablishment = _fixture.Build<Establishment>()
            .With(item => item.IsDeleted, false)
            .Create();

        await establishmentCollection.InsertManyAsync([establishment, anotherEstablishment], cancellation: TestContext.Current.CancellationToken);

        var product = _fixture.Build<Product>()
            .With(item => item.Metadata, new ProductMetadata(anotherEstablishment.Id))
            .With(item => item.IsDeleted, false)
            .Create();

        await productCollection.InsertAsync(product, cancellation: TestContext.Current.CancellationToken);

        var request = _fixture.Build<ProductEditionScheme>()
            .With(item => item.Title, "Produto Atualizado")
            .With(item => item.Description, "Descrição atualizada com fixture")
            .With(item => item.Price, 99.90m)
            .Create();

        var response = await httpClient.PutAsJsonAsync($"/api/v1/establishments/{establishment.Id}/products/{product.Id}", request, _serializerOptions, TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
    }

    [Fact(DisplayName = "[e2e] - when DELETE /api/v1/establishments/{id}/products/{productId} is called and product exists, 204 NoContent is returned")]
    public async Task When_DeleteProduct_IsCalled_AndProductExists_Then_204NoContentIsReturned()
    {
        var httpClient = factory.HttpClient;

        var establishmentCollection = factory.Services.GetRequiredService<IEstablishmentCollection>();
        var productCollection = factory.Services.GetRequiredService<IProductCollection>();

        var establishment = _fixture.Build<Establishment>()
            .With(item => item.IsDeleted, false)
            .Create();

        await establishmentCollection.InsertAsync(establishment, cancellation: TestContext.Current.CancellationToken);

        var product = _fixture.Build<Product>()
            .With(item => item.Metadata, new ProductMetadata(establishment.Id))
            .With(item => item.IsDeleted, false)
            .Create();

        await productCollection.InsertAsync(product, cancellation: TestContext.Current.CancellationToken);

        var response = await httpClient.DeleteAsync($"/api/v1/establishments/{establishment.Id}/products/{product.Id}", TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        var filters = ProductFilters.WithSpecifications()
            .WithIdentifier(product.Id)
            .Build();

        var deletedProducts = await productCollection.FilterProductsAsync(filters, TestContext.Current.CancellationToken);

        Assert.Empty(deletedProducts);
    }

    [Fact(DisplayName = "[e2e] - when POST /api/v1/establishments/{id}/integrations/credentials is called and establishment exists, 201 Created is returned")]
    public async Task When_AssignIntegrationCredential_IsCalled_AndEstablishmentExists_Then_201CreatedIsReturned()
    {
        var httpClient = factory.HttpClient;
        var collection = factory.Services.GetRequiredService<IEstablishmentCollection>();

        var establishment = _fixture.Build<Establishment>()
            .With(item => item.IsDeleted, false)
            .Create();

        await collection.InsertAsync(establishment, cancellation: TestContext.Current.CancellationToken);

        var request = _fixture.Build<CredentialCreationScheme>()
            .With(item => item.Provider, IntegrationTarget.PaymentGateway)
            .With(item => item.SecretKey, "credential-secret")
            .Create();

        var response = await httpClient.PostAsJsonAsync($"/api/v1/establishments/{establishment.Id}/integrations/credentials", request, _serializerOptions, TestContext.Current.CancellationToken);
        var content = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.False(string.IsNullOrWhiteSpace(content));

        var result = JsonSerializer.Deserialize<CredentialScheme>(content, _serializerOptions);

        Assert.NotNull(result);
        Assert.Equal(request.SecretKey, result.SecretKey);
    }

    [Fact(DisplayName = "[e2e] - when POST /api/v1/establishments/{id}/integrations/credentials is called and establishment does not exist, 404 NotFound is returned")]
    public async Task When_AssignIntegrationCredential_IsCalled_AndEstablishmentDoesNotExist_Then_404NotFoundIsReturned()
    {
        var httpClient = factory.HttpClient;

        var request = _fixture.Build<CredentialCreationScheme>()
            .With(item => item.Provider, IntegrationTarget.PaymentGateway)
            .With(item => item.SecretKey, "credential-secret")
            .Create();

        var response = await httpClient.PostAsJsonAsync("/api/v1/establishments/non-existent-id/integrations/credentials", request, _serializerOptions, TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact(DisplayName = "[e2e] - when PUT /api/v1/establishments/{id}/integrations/credentials/{credentialId} is called and data is valid, 200 OK is returned")]
    public async Task When_UpdateCredential_IsCalled_AndDataIsValid_Then_200OkIsReturned()
    {
        var httpClient = factory.HttpClient;

        var establishmentCollection = factory.Services.GetRequiredService<IEstablishmentCollection>();
        var credentialCollection = factory.Services.GetRequiredService<ICredentialCollection>();

        var credential = _fixture.Build<Credential>()
            .With(item => item.Provider, IntegrationTarget.PaymentGateway)
            .With(item => item.SecretKey, "old-secret")
            .With(item => item.IsDeleted, false)
            .Create();

        await credentialCollection.InsertAsync(credential, cancellation: TestContext.Current.CancellationToken);

        var establishment = _fixture.Build<Establishment>()
            .With(item => item.Credentials, [credential])
            .With(item => item.IsDeleted, false)
            .Create();

        await establishmentCollection.InsertAsync(establishment, cancellation: TestContext.Current.CancellationToken);

        var request = _fixture.Build<CredentialEditScheme>()
            .With(item => item.Provider, IntegrationTarget.Whatsapp)
            .With(item => item.SecretKey, "credential-secret-updated")
            .Create();

        var response = await httpClient.PutAsJsonAsync($"/api/v1/establishments/{establishment.Id}/integrations/credentials/{credential.Id}", request, _serializerOptions, TestContext.Current.CancellationToken);
        var content = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.False(string.IsNullOrWhiteSpace(content));

        var result = JsonSerializer.Deserialize<CredentialScheme>(content, _serializerOptions);

        Assert.NotNull(result);
        Assert.Equal(request.SecretKey, result.SecretKey);
    }

    [Fact(DisplayName = "[e2e] - when PUT /api/v1/establishments/{id}/integrations/credentials/{credentialId} is called and establishment does not exist, 404 NotFound is returned")]
    public async Task When_UpdateCredential_IsCalled_AndEstablishmentDoesNotExist_Then_404NotFoundIsReturned()
    {
        var httpClient = factory.HttpClient;

        var request = _fixture.Build<CredentialEditScheme>()
            .With(item => item.Provider, IntegrationTarget.Whatsapp)
            .With(item => item.SecretKey, "credential-secret-updated")
            .Create();

        var response = await httpClient.PutAsJsonAsync("/api/v1/establishments/non-existent-id/integrations/credentials/non-existent-credential", request, _serializerOptions, TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact(DisplayName = "[e2e] - when PUT /api/v1/establishments/{id}/integrations/credentials/{credentialId} is called and credential does not exist, 404 NotFound is returned")]
    public async Task When_UpdateCredential_IsCalled_AndCredentialDoesNotExist_Then_404NotFoundIsReturned()
    {
        var httpClient = factory.HttpClient;
        var establishmentCollection = factory.Services.GetRequiredService<IEstablishmentCollection>();

        var establishment = _fixture.Build<Establishment>()
            .With(item => item.IsDeleted, false)
            .Create();

        await establishmentCollection.InsertAsync(establishment, cancellation: TestContext.Current.CancellationToken);

        var request = _fixture.Build<CredentialEditScheme>()
            .With(item => item.Provider, IntegrationTarget.Whatsapp)
            .With(item => item.SecretKey, "credential-secret-updated")
            .Create();

        var response = await httpClient.PutAsJsonAsync($"/api/v1/establishments/{establishment.Id}/integrations/credentials/non-existent-credential", request, _serializerOptions, TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    public ValueTask InitializeAsync() => factory.InitializeAsync();
    public ValueTask DisposeAsync() => factory.DisposeAsync();
}
