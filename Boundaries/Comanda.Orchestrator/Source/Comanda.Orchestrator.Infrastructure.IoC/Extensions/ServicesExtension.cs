namespace Comanda.Orchestrator.Infrastructure.IoC.Extensions;

[ExcludeFromCodeCoverage(Justification = "contains only dependency injection registration with no business logic.")]
public static class ServicesExtension
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSettings(configuration);
        services.AddMediator();
        services.AddValidation();
        services.AddGateways();
    }
}
