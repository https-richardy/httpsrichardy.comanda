namespace Comanda.Subscriptions.TestSuite.Endpoints;

public sealed class SubscriptionEndpointTests(IntegrationEnvironmentFixture factory) :
    IClassFixture<IntegrationEnvironmentFixture>,
    IAsyncLifetime
{
    private readonly Fixture _fixture = new();
    private readonly JsonSerializerOptions _serializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    [Fact(DisplayName = "[e2e] - when GET /api/v1/subscriptions is called and there are subscriptions, 200 OK is returned with the subscriptions")]
    public async Task When_GetSubscriptions_IsCalled_AndThereAreSubscriptions_Then_200OkIsReturnedWithTheSubscriptions()
    {
        /* arrange: resolve http client and collection instances from integration environment */
        var httpClient = factory.HttpClient;
        var collection = factory.Services.GetRequiredService<ISubscriptionCollection>();

        /* arrange: create 3 active subscriptions to be inserted in the database */
        var subscriptions = _fixture.Build<Subscription>()
            .With(subscription => subscription.Status, Status.Active)
            .With(subscription => subscription.Metadata, SubscriptionMetadata.None)
            .CreateMany(3)
            .ToList();

        await collection.InsertManyAsync(subscriptions, cancellation: TestContext.Current.CancellationToken);

        /* act: send GET request to the subscriptions endpoint */
        var response = await httpClient.GetAsync("/api/v1/subscriptions", TestContext.Current.CancellationToken);
        var content = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);

        /* assert: verify http response status and content */
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.False(string.IsNullOrWhiteSpace(content));

        /* assert: deserialize response and validate data structure */
        var result = JsonSerializer.Deserialize<PaginationScheme<SubscriptionScheme>>(content, _serializerOptions);

        /* assert: ensure response contains subscriptions */
        Assert.NotNull(result);
        Assert.NotEmpty(result.Items);

        /* assert: check if the number of returned subscriptions matches the inserted ones */
        Assert.Equal(subscriptions.Count, result.Items.Count);
    }

    [Fact(DisplayName = "[e2e] - when POST /api/v1/subscriptions is called with valid data, 200 OK is returned with checkout session url")]
    public async Task When_CreateCheckoutSession_IsCalled_WithValidData_Then_200OkIsReturnedWithCheckoutUrl()
    {
        /* arrange: resolve http client instance from integration environment */
        var httpClient = factory.HttpClient;

        /* arrange: create a fake checkout session creation payload */
        var payload = _fixture.Build<CheckoutSessionCreationScheme>()
            .With(payload => payload.Plan, Plan.Basic)
            .Create();

        /* act: send POST request to create checkout session endpoint */
        var response = await httpClient.PostAsJsonAsync("/api/v1/subscriptions", payload, TestContext.Current.CancellationToken);
        var content = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);

        /* assert: verify http response status and content */
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.False(string.IsNullOrWhiteSpace(content));

        /* assert: deserialize response and validate data structure */
        var result = JsonSerializer.Deserialize<CheckoutSession>(content, _serializerOptions);

        /* assert: ensure a valid checkout url was returned */
        Assert.NotNull(result);
        Assert.False(string.IsNullOrWhiteSpace(result.Url));
        Assert.StartsWith("https://checkout.stripe.com/pay/", result.Url);
    }

    [Fact(DisplayName = "[e2e] - when DELETE /api/v1/subscriptions/{id} is called for an active subscription, 200 OK is returned with the canceled subscription")]
    public async Task When_CancelSubscription_IsCalled_ForActiveSubscription_Then_200OkIsReturnedWithCanceledSubscription()
    {
        /* arrange: resolve http client and collection instances from integration environment */
        var httpClient = factory.HttpClient;
        var collection = factory.Services.GetRequiredService<ISubscriptionCollection>();

        /* arrange: create and insert a single active subscription */
        var subscription = _fixture.Build<Subscription>()
            .With(subscription => subscription.Status, Status.Active)
            .With(subscription => subscription.Metadata, SubscriptionMetadata.None)
            .Create();

        await collection.InsertAsync(subscription, cancellation: TestContext.Current.CancellationToken);

        /* act: send DELETE request to cancel the subscription */
        var response = await httpClient.DeleteAsync($"/api/v1/subscriptions/{subscription.Id}", TestContext.Current.CancellationToken);
        var content = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);

        /* assert: verify http response status and content */
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.False(string.IsNullOrWhiteSpace(content));

        /* assert: deserialize response and validate data structure */
        var result = JsonSerializer.Deserialize<Subscription>(content, _serializerOptions);

        /* assert: ensure returned subscription is canceled */
        Assert.NotNull(result);
        Assert.Equal(Status.Canceled, result.Status);
    }

    [Fact(DisplayName = "[e2e] - when DELETE /api/v1/subscriptions/{id} is called for a canceled subscription, 422 is returned")]
    public async Task When_CancelSubscription_IsCalled_ForAlreadyCanceledSubscription_Then_422UnprocessableEntityIsReturned()
    {
        /* arrange: resolve http client instance from integration environment */
        var httpClient = factory.HttpClient;

        /* act: send DELETE request to cancel the same subscription again */
        var response = await httpClient.DeleteAsync($"/api/v1/subscriptions/subscription.canceled", TestContext.Current.CancellationToken);
        var content = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);

        /* assert: verify http response status and content */
        Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
        Assert.False(string.IsNullOrWhiteSpace(content));

        /* assert: deserialize and ensure error matches #COMANDA-ERROR-06F07 */
        var error = JsonSerializer.Deserialize<Error>(content, _serializerOptions);

        Assert.NotNull(error);
        Assert.Equal(SubscriptionErrors.SubscriptionAlreadyCanceled, error);
    }

    public ValueTask InitializeAsync() => factory.InitializeAsync();
    public ValueTask DisposeAsync() => factory.DisposeAsync();
}
