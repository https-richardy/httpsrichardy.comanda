namespace Comanda.Merchants.WebUI;

internal static class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        var configuration = builder.Configuration;

        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");

        builder.Services.AddMudServices();
        builder.Services.AddInfrastructure(configuration);

        var host = builder.Build();

        await host.RunAsync();
    }
}
