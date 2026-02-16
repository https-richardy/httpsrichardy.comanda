namespace Comanda.Subscriptions.Infrastructure.IoC.Extensions;

[ExcludeFromCodeCoverage(Justification = "contains only dependency injection registration with no business logic.")]
public static class ServicesExtension
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var settings = services.AddSettings(configuration);

        services.AddDataPersistence(settings);
        services.AddServices(settings);

        services.AddMediator();
        services.AddValidation();
    }
}