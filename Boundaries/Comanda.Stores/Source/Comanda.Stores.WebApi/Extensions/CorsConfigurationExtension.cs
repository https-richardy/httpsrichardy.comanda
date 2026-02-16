namespace Comanda.Stores.WebApi.Extensions;

[ExcludeFromCodeCoverage(Justification = "contains only CORS configuration with no business logic.")]
public static class CorsConfigurationExtension
{
    public static void AddCorsConfiguration(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.AllowAnyHeader();
                policy.AllowAnyMethod();
                policy.AllowAnyOrigin();
            });
        });
    }
}
