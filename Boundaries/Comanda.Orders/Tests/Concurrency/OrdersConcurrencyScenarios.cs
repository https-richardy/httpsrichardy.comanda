namespace Comanda.Orders.TestSuite.Concurrency;

public sealed class OrdersConcurrencyScenarios(IntegrationEnvironmentFixture factory) :
    IClassFixture<IntegrationEnvironmentFixture>,
    IAsyncLifetime
{
    private readonly Fixture _fixture = new();
    private readonly JsonSerializerOptions _serializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    [Fact(DisplayName = "[concurrency] - when multiple clients update the same order concurrently, at least 80% of updates succeed")]
    public async Task When_MultipleClientsUpdateSameOrder_Concurrently_Then_AtLeast80PercentSucceed()
    {
        /* arrange: resolve http client and repository instances from integration environment */
        var httpClient = factory.HttpClient;
        var repository = factory.Services.GetRequiredService<IOrderCollection>();

        /* arrange: create an existing order to update */
        var existingOrder = _fixture.Build<Order>()
            .With(order => order.IsDeleted, false)
            .With(order => order.Status, Status.Pending)
            .With(order => order.Priority, Priority.Normal)
            .Create();

        await repository.InsertAsync(existingOrder, cancellation: TestContext.Current.CancellationToken);

        /* arrange: define number of concurrent updates */
        const int concurrentUpdates = 50;
        const double minimumSuccessRate = 0.80;

        /* arrange: prepare concurrent update requests with different priorities */
        var tasks = new List<Task<HttpResponseMessage>>();

        /* act: simulate concurrent updates from multiple clients */
        for (var index = 0; index < concurrentUpdates; index++)
        {
            var request = _fixture.Build<OrderModificationScheme>()
                .Without(order => order.Id)
                .With(order => order.Status, Status.InPreparation)
                .With(order => order.Priority, (Priority)(index % 3))
                .Create();

            var task = httpClient.PutAsJsonAsync($"/api/v1/orders/{existingOrder.Id}", request, TestContext.Current.CancellationToken);

            tasks.Add(task);
        }

        var responses = await Task.WhenAll(tasks);

        /* assert: count successful updates */
        var successCount = responses.Count(response => response.StatusCode == HttpStatusCode.OK);
        var successRate = (double)successCount / concurrentUpdates;

        /* assert: verify at least 80% of updates succeeded */
        Assert.True(successRate >= minimumSuccessRate,
            $"success rate was {successRate:P}, expected at least {minimumSuccessRate:P}. " +
            $"successful updates: {successCount}/{concurrentUpdates}");

        /* assert: verify final state is consistent via API */
        var response = await httpClient.GetAsync($"/api/v1/orders?id={existingOrder.Id}", TestContext.Current.CancellationToken);
        var content = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);

        var orders = JsonSerializer.Deserialize<IEnumerable<OrderScheme>>(content, _serializerOptions);

        /* assert: verifies that a non-null and non-empty orders collection was returned */
        Assert.NotNull(orders);
        Assert.NotEmpty(orders);

        var order = orders.FirstOrDefault();

        /* assert: verifies that the returned collection contains an order with the expected status */
        Assert.NotNull(order);
        Assert.Equal(Status.InPreparation, order.Status);
    }

    [Fact(DisplayName = "[concurrency] - when 1000 orders are created simultaneously, 100% are created with unique codes and full integrity")]
    public async Task When_1000OrdersCreated_Simultaneously_Then_100PercentCreated_WithUniqueCodesAndFullIntegrity()
    {
        /* arrange: resolve http client instance from integration environment */
        var httpClient = factory.HttpClient;

        /* arrange: define extreme concurrent load for stress testing */
        const int totalConcurrentCreations = 1000;
        const double requiredSuccessRate = 1.00;

        var tasks = new List<Task<HttpResponseMessage>>();
        var expectedOrders = new List<OrderCreationScheme>();

        /* act: simulate 1000 concurrent order creations from multiple clients */
        for (var index = 0; index < totalConcurrentCreations; index++)
        {
            var request = _fixture.Build<OrderCreationScheme>()
                .With(order => order.Metadata, _fixture.Create<Metadata>())
                .With(order => order.Items, _fixture.CreateMany<Item>(3))
                .With(order => order.Priority, (Priority)(index % 3))
                .With(order => order.Fulfillment, (Fulfillment)(index % 3))
                .Create();

            expectedOrders.Add(request);

            var task = httpClient.PostAsJsonAsync("/api/v1/orders", request, TestContext.Current.CancellationToken);

            tasks.Add(task);
        }

        var responses = await Task.WhenAll(tasks);

        /* assert: verify 100% success rate */
        var successfulResponses = responses.Where(response => response.StatusCode == HttpStatusCode.Created).ToList();
        var successRate = (double)successfulResponses.Count / totalConcurrentCreations;

        var failedCount = totalConcurrentCreations - successfulResponses.Count;

        Assert.True(successRate >= requiredSuccessRate,
            $"success rate was {successRate:P}, expected 100%. " +
            $"successful creations: {successfulResponses.Count}/{totalConcurrentCreations}. " +
            $"failed: {failedCount}"
        );

        /* assert: extract and validate all created orders */
        var createdOrders = new List<OrderScheme>();

        foreach (var response in successfulResponses)
        {
            var content = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
            var order = JsonSerializer.Deserialize<OrderScheme>(content, _serializerOptions);

            Assert.NotNull(order);

            createdOrders.Add(order);
        }

        /* assert: verify all order identifiers are unique */
        var uniqueIds = createdOrders.Select(order => order.Identifier)
            .Distinct()
            .Count();

        var duplicateIds = totalConcurrentCreations - uniqueIds;

        Assert.True(uniqueIds == totalConcurrentCreations,
            $"found {duplicateIds} duplicate IDs. unique: {uniqueIds}/{totalConcurrentCreations}");

        /* assert: verify all order codes are unique */
        var uniqueCodes = createdOrders.Select(order => order.Code)
            .Distinct()
            .Count();

        var duplicateCodes = totalConcurrentCreations - uniqueCodes;

        Assert.True(uniqueCodes == totalConcurrentCreations,
            $"found {duplicateCodes} duplicate codes. unique: {uniqueCodes}/{totalConcurrentCreations}");

        /* assert: verify no empty or null codes */
        Assert.All(createdOrders, order =>
        {
            Assert.False(string.IsNullOrWhiteSpace(order.Code), $"order {order.Identifier} has null or empty code");
        });

        /* assert: verify no empty or null identifiers */
        Assert.All(createdOrders, order =>
        {
            Assert.False(string.IsNullOrWhiteSpace(order.Identifier), "found order with null or empty identifier");
        });

        /* assert: verify all orders have valid metadata */
        Assert.All(createdOrders, order =>
        {
            Assert.NotNull(order.Metadata);

            Assert.False(string.IsNullOrWhiteSpace(order.Metadata.MerchantId),
                $"order {order.Identifier} has invalid MerchantId");

            Assert.False(string.IsNullOrWhiteSpace(order.Metadata.ConsumerId),
                $"order {order.Identifier} has invalid ConsumerId");
        });

        /* assert: verify all orders have items */
        Assert.All(createdOrders, order =>
        {
            Assert.NotNull(order.Items);
            Assert.NotEmpty(order.Items);

            Assert.Equal(3, order.Items.Count());
        });

        /* assert: verify all orders have valid status, priority and fulfillment */
        Assert.All(createdOrders, order =>
        {
            Assert.True(Enum.IsDefined(order.Status),
                $"order {order.Identifier} has invalid status: {order.Status}");

            Assert.True(Enum.IsDefined(order.Priority),
                $"order {order.Identifier} has invalid priority: {order.Priority}");

            Assert.True(Enum.IsDefined(order.Fulfillment),
                $"order {order.Identifier} has invalid fulfillment: {order.Fulfillment}");
        });

        /* assert: verify created orders exist in database via API for first 100 orders */
        for (var index = 0; index < Math.Min(100, createdOrders.Count); index++)
        {
            var orderId = createdOrders[index].Identifier;
            var verifyResponse = await httpClient.GetAsync($"/api/v1/orders?id={orderId}", TestContext.Current.CancellationToken);

            var verifyContent = await verifyResponse.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
            var verifyOrders = JsonSerializer.Deserialize<IEnumerable<OrderScheme>>(verifyContent, _serializerOptions);

            Assert.NotNull(verifyOrders);
            Assert.Contains(verifyOrders, order => order.Identifier == orderId);
        }

        /* assert: verify data consistency by comparing created orders with expected data for first 50 */
        for (var index = 0; index < Math.Min(50, createdOrders.Count); index++)
        {
            var created = createdOrders[index];
            var expected = expectedOrders[index];

            Assert.Equal(expected.Priority, created.Priority);
            Assert.Equal(expected.Fulfillment, created.Fulfillment);
            Assert.Equal(expected.Metadata.MerchantId, created.Metadata.MerchantId);

            Assert.Equal(expected.Metadata.ConsumerId, created.Metadata.ConsumerId);
            Assert.Equal(expected.Items.Count(), created.Items.Count());
        }

        /* assert: verify total count via GET all orders */
        var getAllResponse = await httpClient.GetAsync("/api/v1/orders", TestContext.Current.CancellationToken);
        var getAllContent = await getAllResponse.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);

        var orders = JsonSerializer.Deserialize<IEnumerable<OrderScheme>>(getAllContent, _serializerOptions);

        Assert.NotNull(orders);
        Assert.True(orders.Count() >= totalConcurrentCreations,
            $"database contains {orders.Count()} orders, expected at least {totalConcurrentCreations}");
    }

    public ValueTask InitializeAsync() => factory.InitializeAsync();
    public ValueTask DisposeAsync() => factory.DisposeAsync();
}
