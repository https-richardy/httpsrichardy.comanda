namespace Comanda.Merchants.WebUI.Extensions;

public static class ServicesExtension
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddClients(configuration);
        services.AddGateways(configuration);
        services.AddAuthentication();
    }
}
