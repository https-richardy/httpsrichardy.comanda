namespace Comanda.Orchestrator.WebApi.Extensions;

[ExcludeFromCodeCoverage(Justification = "contains only web infrastructure configuration with no business logic.")]
public static class WebInfrastructureExtension
{
    public static void AddWebComposition(this IServiceCollection services)
    {
        var provider = services.BuildServiceProvider();
        var settings = provider.GetRequiredService<ISettings>();

        services.AddControllers();
        services.AddHttpContextAccessor();
        services.AddProviders();
        services.AddEndpointsApiExplorer();
        services.AddCorsConfiguration();
        services.AddHttpClients();
        services.AddFluentValidationAutoValidation(options =>
        {
            options.DisableDataAnnotationsValidation = true;
        });

        services.AddOpenApiSpecification();
        services.AddFederation(options =>
        {
            options.BaseUrl = settings.Federation.BaseUrl;
            options.ClientId = settings.Federation.ClientId;
            options.Realm = settings.Federation.Realm;
            options.ClientSecret = settings.Federation.ClientSecret;
        });
    }
}
