namespace Comanda.Internal.Contracts.Tests.Fixtures;

public sealed class WireMockFixture : IAsyncLifetime
{
    public WireMockServer WireMock { get; private set; }
    public HttpClient HttpClient { get; private set; }

    public ValueTask InitializeAsync()
    {
        WireMock = WireMockServer.Start();
        HttpClient = new HttpClient
        {
            BaseAddress = new Uri($"{WireMock.Url}/")
        };

        return ValueTask.CompletedTask;
    }

    public ValueTask DisposeAsync()
    {
        HttpClient.Dispose();

        WireMock.Stop();
        WireMock.Dispose();

        return ValueTask.CompletedTask;
    }
}
