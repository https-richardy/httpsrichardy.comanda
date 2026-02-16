namespace Comanda.Stores.WebApi.Extensions;

[ExcludeFromCodeCoverage(Justification = "contains only web infrastructure configuration with no business logic.")]
public static class WebInfrastructureExtension
{
    public static void AddWebComposition(this IServiceCollection services, IWebHostEnvironment environment)
    {
        var provider = services.BuildServiceProvider();
        var settings = provider.GetRequiredService<ISettings>();

        services.AddControllers();
        services.AddHttpContextAccessor();
        services.AddEndpointsApiExplorer();
        services.AddCorsConfiguration();
        services.AddGateways(environment);

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
