namespace Comanda.Subscriptions.TestSuite.Endpoints;

public sealed class CallbacksEndpointTests(IntegrationEnvironmentFixture factory) :
    IClassFixture<IntegrationEnvironmentFixture>,
    IAsyncLifetime
{
    private readonly Fixture _fixture = new();
    private readonly JsonSerializerOptions _serializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    [Fact(DisplayName = "[e2e] - when GET /api/v1/callback/success is called, 200 OK is returned with the updated subscription")]
    public async Task When_OnSuccessCallback_IsCalled_Then_200OkIsReturnedWithUpdatedSubscription()
    {
        /* arrange: resolve http client and collection instances from integration environment */
        var httpClient = factory.HttpClient;
        var collection = factory.Services.GetRequiredService<ISubscriptionCollection>();

        /* arrange: create a subscription that will be marked active by the callback */
        var subscription = _fixture.Build<Subscription>()
            .With(subscription => subscription.Status, Status.None)
            .With(subscription => subscription.Metadata, SubscriptionMetadata.None)
            .Create();

        await collection.InsertAsync(subscription, cancellation: TestContext.Current.CancellationToken);

        /* act: send GET request to the success callback endpoint */
        var response = await httpClient.GetAsync("/api/v1/callback/success?sessionId=session_XXXXXXXXX", TestContext.Current.CancellationToken);
        var content = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);

        /* assert: verify http response status and content */
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.False(string.IsNullOrWhiteSpace(content));

        /* assert: deserialize response and ensure subscription is active */
        var result = JsonSerializer.Deserialize<Subscription>(content, _serializerOptions);

        Assert.NotNull(result);
        Assert.Equal(Status.Active, result.Status);
    }

    [Fact(DisplayName = "[e2e] - when GET /api/v1/callback/cancel is called, 200 OK is returned with the failed subscription")]
    public async Task When_OnCancelCallback_IsCalled_Then_200OkIsReturnedWithFailedSubscription()
    {
        /* arrange: resolve http client and collection instances from integration environment */
        var httpClient = factory.HttpClient;
        var collection = factory.Services.GetRequiredService<ISubscriptionCollection>();

        /* arrange: create a subscription that will be failed by the callback */
        var subscription = _fixture.Build<Subscription>()
            .With(subscription => subscription.Status, Status.Active)
            .With(subscription => subscription.Metadata, SubscriptionMetadata.None)
            .Create();

        await collection.InsertAsync(subscription, cancellation: TestContext.Current.CancellationToken);

        var response = await httpClient.GetAsync("/api/v1/callback/cancel?sessionId=session_XXXXXXXXX", TestContext.Current.CancellationToken);
        var content = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.False(string.IsNullOrWhiteSpace(content));

        var result = JsonSerializer.Deserialize<Subscription>(content, _serializerOptions);

        Assert.NotNull(result);
        Assert.Equal(Status.None, result.Status);
    }

    public ValueTask InitializeAsync() => factory.InitializeAsync();
    public ValueTask DisposeAsync() => factory.DisposeAsync();
}
