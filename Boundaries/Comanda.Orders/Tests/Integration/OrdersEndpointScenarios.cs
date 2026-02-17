namespace Comanda.Orders.TestSuite.Integration;

public sealed class OrdersEndpointScenarios(IntegrationEnvironmentFixture factory) :
    IClassFixture<IntegrationEnvironmentFixture>,
    IAsyncLifetime
{
    private readonly Fixture _fixture = new();
    private readonly JsonSerializerOptions _serializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    [Fact(DisplayName = "[e2e] - when GET /api/v1/orders is called and there are orders, 200 OK is returned with the orders")]
    public async Task When_GetOrders_IsCalled_AndThereAreOrders_Then_200OkIsReturnedWithTheOrders()
    {
        /* arrange: resolves http client and repository instances from integration environment */
        var httpClient = factory.HttpClient;
        var repository = factory.Services.GetRequiredService<IOrderCollection>();

        /* arrange: create 3 non-deleted orders to be inserted in the database */
        var orders = _fixture.Build<Order>()
            .With(order => order.IsDeleted, false)
            .CreateMany(3)
            .ToList();

        await repository.InsertManyAsync(orders, cancellation: TestContext.Current.CancellationToken);

        /* act: send GET request to the orders endpoint */
        var response = await httpClient.GetAsync("/api/v1/orders", TestContext.Current.CancellationToken);
        var content = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);

        /* assert: verify http response status and content */
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.False(string.IsNullOrWhiteSpace(content));

        /* assert: deserialize response and validate data structure */
        var result = JsonSerializer.Deserialize<IEnumerable<OrderScheme>>(content, _serializerOptions);

        /* assert: ensure response contains orders */
        Assert.NotNull(result);
        Assert.NotEmpty(result);

        /* assert: check if the number of returned orders matches the inserted ones */
        Assert.Equal(orders.Count, result.Count());
    }

    [Fact(DisplayName = "[e2e] - when POST /api/v1/orders is called with valid data, 201 Created is returned with the created order")]
    public async Task When_CreateOrder_IsCalled_WithValidData_Then_201CreatedIsReturnedWithTheCreatedOrder()
    {
        /* arrange: resolve http client instance from integration environment */
        var httpClient = factory.HttpClient;

        /* arrange: create an order creation scheme using fixture */
        var request = _fixture.Build<OrderCreationScheme>()
            .With(order => order.Metadata, _fixture.Create<Metadata>())
            .With(order => order.Items, _fixture.CreateMany<Item>(2))
            .Create();

        /* act: send POST request to the orders endpoint */
        var response = await httpClient.PostAsJsonAsync("/api/v1/orders", request, TestContext.Current.CancellationToken);
        var content = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);

        /* assert: verify http status code */
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.False(string.IsNullOrWhiteSpace(content));

        /* assert: deserialize response to OrderScheme */
        var result = JsonSerializer.Deserialize<OrderScheme>(content, _serializerOptions);

        /* assert: ensure response is not null */
        Assert.NotNull(result);

        /* assert: compare basic fields between request and result */
        Assert.Equal(request.Fulfillment, result.Fulfillment);
        Assert.Equal(request.Priority, result.Priority);

        Assert.Equal(request.Metadata.MerchantId, result.Metadata.MerchantId);
        Assert.Equal(request.Metadata.ConsumerId, result.Metadata.ConsumerId);

        /* assert: compare items count */
        Assert.Equal(request.Items.Count(), result.Items.Count());
    }

    [Fact(DisplayName = "[e2e] - when PUT /api/v1/orders/{id} is called with valid data, 200 OK is returned with the updated order")]
    public async Task When_UpdateOrder_IsCalled_WithValidData_Then_200OkIsReturnedWithTheUpdatedOrder()
    {
        /* arrange: resolve http client and repository instances from integration environment */
        var httpClient = factory.HttpClient;
        var repository = factory.Services.GetRequiredService<IOrderCollection>();

        /* arrange: create an existing order to update */
        var existingOrder = _fixture.Build<Order>()
            .With(order => order.IsDeleted, false)
            .Create();

        await repository.InsertAsync(existingOrder, cancellation: TestContext.Current.CancellationToken);

        /* arrange: create modification scheme without id (id is taken from route) */
        var request = _fixture.Build<OrderModificationScheme>()
            .Without(order => order.Id)
            .Create();

        /* act: send PUT request to the orders endpoint */
        var response = await httpClient.PutAsJsonAsync($"/api/v1/orders/{existingOrder.Id}", request, TestContext.Current.CancellationToken);
        var content = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);

        /* assert: verify http status code */
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.False(string.IsNullOrWhiteSpace(content));

        /* assert: deserialize response to OrderScheme */
        var result = JsonSerializer.Deserialize<OrderScheme>(content, _serializerOptions);

        /* assert: ensure response is not null */
        Assert.NotNull(result);

        /* assert: verify updated properties */
        Assert.Equal(request.Status, result.Status);
        Assert.Equal(request.Priority, result.Priority);

        /* assert: ensure unchanged fields still exist */
        Assert.Equal(existingOrder.Id, result.Identifier);
        Assert.Equal(existingOrder.Fulfillment, result.Fulfillment);

        Assert.Equal(existingOrder.Metadata.MerchantId, result.Metadata.MerchantId);
        Assert.Equal(existingOrder.Metadata.ConsumerId, result.Metadata.ConsumerId);
    }

    [Fact(DisplayName = "[e2e] - when PUT /api/v1/orders/{id} is called for a non-existent order, 404 NotFound is returned")]
    public async Task When_UpdateOrder_IsCalled_ForNonExistentOrder_Then_404NotFoundIsReturned()
    {
        /* arrange: resolve http client instance from integration environment */
        var httpClient = factory.HttpClient;

        /* arrange: generate a random order id that does not exist in the database */
        var nonExistentOrderId = Identifier.Generate<Order>();

        /* arrange: create modification scheme without id (id is taken from route) */
        var request = _fixture.Build<OrderModificationScheme>()
            .Without(order => order.Id)
            .Create();

        /* act: send PUT request to the orders endpoint */
        var response = await httpClient.PutAsJsonAsync($"/api/v1/orders/{nonExistentOrderId}", request, TestContext.Current.CancellationToken);
        var content = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);

        /* assert: verify http status code */
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        /* assert: body should contain error details */
        Assert.False(string.IsNullOrWhiteSpace(content));

        /* assert: ensure error code matches the expected domain error */
        var error = JsonSerializer.Deserialize<Error>(content, _serializerOptions);

        Assert.NotNull(error);
        Assert.Equal(OrderErrors.OrderDoesNotExist, error);
    }

    public ValueTask InitializeAsync() => factory.InitializeAsync();
    public ValueTask DisposeAsync() => factory.DisposeAsync();
}
