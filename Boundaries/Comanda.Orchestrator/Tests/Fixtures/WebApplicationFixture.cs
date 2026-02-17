using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace Comanda.Orchestrator.TestSuite.Fixtures;

public sealed class WebApplicationFixture : IAsyncLifetime
{
    public HttpClient HttpClient { get; private set; } = default!;
    public IServiceProvider Services { get; private set; } = default!;
    private WebApplicationFactory<Program> _factory = default!;

    public async ValueTask InitializeAsync()
    {
        _factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var policy = services.AddAuthentication(options =>
                    {
                        options.DefaultAuthenticateScheme = Defaults.AuthenticationScheme;
                        options.DefaultChallengeScheme = Defaults.AuthenticationScheme;
                        options.DefaultScheme = Defaults.AuthenticationScheme;
                    });

                    policy.AddScheme<AuthenticationSchemeOptions, BypassAuthenticationHandler>(Defaults.AuthenticationScheme, _ => {  });
                    services.AddAuthorizationBuilder()
                        .SetDefaultPolicy(new AuthorizationPolicyBuilder(Defaults.AuthenticationScheme)
                            .RequireAuthenticatedUser()
                            .Build());
                });
            });

        HttpClient = _factory.CreateClient();
        Services = _factory.Services;
    }

    public async ValueTask DisposeAsync()
    {
        HttpClient.Dispose();

        await _factory.DisposeAsync();
    }
}
