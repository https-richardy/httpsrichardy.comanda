namespace Comanda.Orchestrator.TestSuite.Fixtures;

public sealed class IntegrationEnvironmentFixture : IAsyncLifetime
{
    private readonly WebApplicationFixture _factory;

    public HttpClient HttpClient => _factory.HttpClient;
    public IServiceProvider Services => _factory.Services;

    public IntegrationEnvironmentFixture()
    {
        _factory = new WebApplicationFixture();
    }

    public async ValueTask InitializeAsync()
    {
        await _factory.InitializeAsync();
    }

    public async ValueTask DisposeAsync()
    {
        await _factory.DisposeAsync();
    }
}

