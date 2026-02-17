namespace Comanda.Orchestrator.WebApi.Extensions;

[ExcludeFromCodeCoverage(Justification = "contains only dependency injection configuration with no business logic.")]
public static class ProvidersExtension
{
    public static void AddProviders(this IServiceCollection services)
    {
        services.AddTransient<IPrincipalProvider, PrincipalProvider>();
    }
}
