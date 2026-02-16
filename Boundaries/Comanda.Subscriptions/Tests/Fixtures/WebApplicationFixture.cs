using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;

namespace Comanda.Subscriptions.TestSuite.Fixtures;

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
                builder.UseEnvironment("Testing");
                builder.ConfigureServices(services =>
                {
                    services.AddAuthentication(options =>
                    {
                        options.DefaultAuthenticateScheme = "vinder.internal.bypass.authentication";
                        options.DefaultChallengeScheme = "vinder.internal.bypass.authentication";
                        options.DefaultScheme = "vinder.internal.bypass.authentication";
                    })
                    .AddScheme<AuthenticationSchemeOptions, BypassAuthenticationHandler>("vinder.internal.bypass.authentication", _ => { });

                    services.AddAuthorization(options =>
                    {
                        options.DefaultPolicy = new AuthorizationPolicyBuilder("vinder.internal.bypass.authentication")
                            .RequireAuthenticatedUser()
                            .Build();
                    });

                    var descriptor = services.SingleOrDefault(descriptor => descriptor.ServiceType == typeof(IMongoClient));
                    if (descriptor is not null)
                        services.Remove(descriptor);

                    descriptor = services.SingleOrDefault(descriptor => descriptor.ServiceType == typeof(IMongoDatabase));
                    if (descriptor is not null)
                        services.Remove(descriptor);

                    descriptor = services.SingleOrDefault(descriptor => descriptor.ServiceType == typeof(ISubscriptionGateway));
                    if (descriptor is not null)
                        services.Remove(descriptor);

                    services.AddSingleton(_ => _databaseFixture.Client);
                    services.AddSingleton(_ => _databaseFixture.Database);
                    services.AddSingleton<ISubscriptionGateway, SubscriptionGatewayMock>();
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
