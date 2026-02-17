namespace Comanda.Orchestrator.WebApi.Extensions;

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

                // expose custom response headers so they can be accessed by browser-based clients (e.g. blazor webAssembly).
                // by default, the browser blocks access to non-safelisted headers (such as X-Pagination),

                // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Access-Control-Expose-Headers

                policy.WithExposedHeaders(Headers.Pagination);
                policy.WithExposedHeaders(Headers.Credential);
            });
        });
    }
}
