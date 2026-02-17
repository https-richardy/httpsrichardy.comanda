namespace Comanda.Orchestrator.WebApi;

public partial class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var configuration = builder.Configuration;

        builder.Services.AddInfrastructure(configuration);
        builder.Services.AddWebComposition();

        builder.Configuration.AddEnvironmentVariables();

        builder.AddObservability();
        builder.AddMonitoring();

        var app = builder.Build();

        app.MapOpenApi();

        app.UseHttpPipeline();
        app.UseSpecification();

        await app.RunAsync();
    }
}
