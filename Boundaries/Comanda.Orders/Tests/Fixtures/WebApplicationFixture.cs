namespace Comanda.Orders.TestSuite.Fixtures;

public sealed class WebApplicationFixture : IAsyncLifetime
{
    private readonly MongoDatabaseFixture _databaseFixture;

    public HttpClient HttpClient { get; private set; } = default!;
    public IServiceProvider Services { get; private set; } = default!;

    private WebApplicationFactory<Program> _factory = default!;

    public WebApplicationFixture()
    {
        _databaseFixture = new MongoDatabaseFixture();
    }

    public async ValueTask InitializeAsync()
    {
        await _databaseFixture.InitializeAsync();

        Environment.SetEnvironmentVariable("Settings__Database__ConnectionString", _databaseFixture.ConnectionString);
        Environment.SetEnvironmentVariable("Settings__Database__DatabaseName", _databaseFixture.DatabaseName);

        _factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddAuthentication(options =>
                    {
                        options.DefaultAuthenticateScheme = "bypass.authentication";
                        options.DefaultChallengeScheme = "bypass.authentication";
                        options.DefaultScheme = "bypass.authentication";
                    })
                    .AddScheme<AuthenticationSchemeOptions, BypassAuthenticationHandler>("bypass.authentication", _ => { });

                    services.AddAuthorization(options =>
                    {
                        options.DefaultPolicy = new AuthorizationPolicyBuilder("bypass.authentication")
                            .RequireAuthenticatedUser()
                            .Build();
                    });

                    var descriptor = services.SingleOrDefault(descriptor => descriptor.ServiceType == typeof(IMongoClient));
                    if (descriptor is not null)
                        services.Remove(descriptor);

                    descriptor = services.SingleOrDefault(descriptor => descriptor.ServiceType == typeof(IMongoDatabase));
                    if (descriptor is not null)
                        services.Remove(descriptor);

                    services.AddSingleton(_ => _databaseFixture.Client);
                    services.AddSingleton(_ => _databaseFixture.Database);
                });
            });

        HttpClient = _factory.CreateClient();
        Services = _factory.Services;
    }

    public async ValueTask DisposeAsync()
    {
        HttpClient.Dispose();

        await _factory.DisposeAsync();
        await _databaseFixture.CleanDatabaseAsync();
        await _databaseFixture.DisposeAsync();
    }
}
