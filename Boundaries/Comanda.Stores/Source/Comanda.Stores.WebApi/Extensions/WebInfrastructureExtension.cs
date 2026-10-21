namespace Comanda.Stores.WebApi.Extensions;

[ExcludeFromCodeCoverage(Justification = "contains only web infrastructure configuration with no business logic.")]
public static class WebInfrastructureExtension
{
    public static void AddWebComposition(this IServiceCollection services, IWebHostEnvironment environment)
    {
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
        services.AddBearerAuthentication();
        services.AddAuthorization();
    }
}
