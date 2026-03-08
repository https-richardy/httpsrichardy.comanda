namespace Comanda.Payments.TestSuite.Integration;

public sealed class PaymentEndpointTests(IntegrationEnvironmentFixture factory) :
    IClassFixture<IntegrationEnvironmentFixture>,
    IAsyncLifetime
{
    private readonly Fixture _fixture = new();
    private readonly JsonSerializerOptions _serializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    [Fact(DisplayName = "[e2e] - when GET /api/v1/payments is called and there are payments, 200 OK is returned with the payments")]
    public async Task GetOwners_ReturnsOwners_WhenOwnersExist()
    {
        /* arrange: resolve http client and collection instances from integration environment */
        var httpClient = factory.HttpClient;
        var collection = factory.Services.GetRequiredService<IPaymentCollection>();

        var payments = _fixture.Build<Payment>()
            .With(payment => payment.IsDeleted, false)
            .CreateMany(3)
            .ToList();

        await collection.InsertManyAsync(payments, cancellation: TestContext.Current.CancellationToken);

        /* act: send GET request to the owners endpoint */
        var response = await httpClient.GetAsync("/api/v1/payments", TestContext.Current.CancellationToken);
        var content = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);

        /* assert: verify http response status and content */
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.False(string.IsNullOrWhiteSpace(content));

        /* assert: deserialize response and validate data structure */
        var result = JsonSerializer.Deserialize<PaginationScheme<PaymentScheme>>(content, _serializerOptions);

        /* assert: ensure response contains owners */
        Assert.NotNull(result);
        Assert.NotEmpty(result.Items);

        /* assert: check if the number of returned payments matches the inserted ones */
        Assert.Equal(payments.Count, result.Items.Count);
    }

    [Fact(DisplayName = "[e2e] - when POST /api/v1/payments/offline is called with valid data, 201 Created is returned and payment is persisted")]
    public async Task CreateOfflinePaymentCharge_ReturnsCreated_AndPersistsPayment()
    {
        /* arrange: resolve http client and collection instances from integration environment */
        var httpClient = factory.HttpClient;
        var collection = factory.Services.GetRequiredService<IPaymentCollection>();

        /* arrange: create valid offline payment charge request */
        var request = _fixture.Build<OfflinePaymentChargeScheme>()
            .With(request => request.Reference, Identifier.Generate<Payment>())
            .With(request => request.Method, Method.Cash)
            .With(request => request.Amount, 100m)
            .Create();

        /* act: send POST request to create offline payment charge */
        var response = await httpClient.PostAsJsonAsync("/api/v1/payments/offline", request, TestContext.Current.CancellationToken);
        var content = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);

        /* assert: verify http response status and content */
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.False(string.IsNullOrWhiteSpace(content));

        /* assert: deserialize response and validate data structure */
        var result = JsonSerializer.Deserialize<PaymentScheme>(content, _serializerOptions);

        /* assert: ensure response contains payment data */
        Assert.NotNull(result);
        Assert.False(string.IsNullOrWhiteSpace(result.Identifier));

        Assert.Equal(request.Reference, result.Reference);
        Assert.Equal(request.Amount, result.Amount);

        Assert.Equal(request.Method, result.Method);
        Assert.Equal(Status.Paid, result.Status);

        /* assert: verify payment was persisted in the database */
        var filters = PaymentFilters.WithSpecifications()
            .WithIdentifier(result.Identifier)
            .Build();

        var persistedPayments = await collection.FilterPaymentsAsync(filters, TestContext.Current.CancellationToken);
        var persistedPayment = persistedPayments.FirstOrDefault();

        Assert.NotNull(persistedPayment);

        Assert.Equal(result.Identifier, persistedPayment.Id);
        Assert.Equal(request.Reference, persistedPayment.Metadata.Reference);

        Assert.Equal(Status.Paid, persistedPayment.Status);
        Assert.Equal(request.Amount, persistedPayment.Amount);
        Assert.Equal(request.Method, persistedPayment.Method);
    }

    [Fact(DisplayName = "[e2e] - when POST /api/v1/payments/online is called with valid data, 200 OK is returned and payment is persisted")]
    public async Task CreateCheckoutSession_ReturnsSuccess_AndPersistsPayment()
    {
        /* arrange: resolve http client and collection instances from integration environment */
        var httpClient = factory.HttpClient;
        var collection = factory.Services.GetRequiredService<IPaymentCollection>();

        /* arrange: create valid checkout session request */
        var request = _fixture.Build<CheckoutSessionCreationScheme>()
            .With(request => request.Reference, Identifier.Generate<Payment>())
            .With(request => request.Amount, 100m)
            .With(request => request.Payer, new User(Identifier.Generate<User>(), $"john.doe@email.com"))
            .Create();

        var payload = JsonSerializer.Serialize(request, _serializerOptions);

        var content = new StringContent(payload, System.Text.Encoding.UTF8, MediaTypeNames.Application.Json);
        var httpRequest = new HttpRequestMessage(HttpMethod.Post, "/api/v1/payments/online")
        {
            Content = content
        };

        httpRequest.Headers.Add(WebApi.Constants.Headers.Credential, "mocked.credential");

        /* act: send POST request to create checkout session */
        var response = await httpClient.SendAsync(httpRequest, TestContext.Current.CancellationToken);
        var responseContent = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);

        /* assert: verify http response status and content */
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.False(string.IsNullOrWhiteSpace(responseContent));

        /* assert: deserialize response and validate session structure */
        var result = JsonSerializer.Deserialize<CheckoutSession>(responseContent, _serializerOptions);

        /* assert: ensure response contains checkout session data */
        Assert.NotNull(result);

        Assert.False(string.IsNullOrWhiteSpace(result.Code));
        Assert.False(string.IsNullOrWhiteSpace(result.QrCode));

        /* assert: verify payment was persisted in the database */
        var filters = PaymentFilters.WithSpecifications()
            .WithReferenceId(request.Reference)
            .Build();

        var persistedPayments = await collection.FilterPaymentsAsync(filters, TestContext.Current.CancellationToken);
        var persistedPayment = persistedPayments.FirstOrDefault();

        Assert.NotNull(persistedPayment);

        Assert.Equal(request.Reference, persistedPayment.Metadata.Reference);
        Assert.Equal(request.Amount, persistedPayment.Amount);

        Assert.Equal(Method.Pix, persistedPayment.Method);
        Assert.Equal(Status.Pending, persistedPayment.Status);

        Assert.Equal(request.Payer.Identifier, persistedPayment.Payer.Identifier);
        Assert.Equal(request.Payer.Username, persistedPayment.Payer.Username);
    }

    public ValueTask InitializeAsync() => factory.InitializeAsync();
    public ValueTask DisposeAsync() => factory.DisposeAsync();
}
