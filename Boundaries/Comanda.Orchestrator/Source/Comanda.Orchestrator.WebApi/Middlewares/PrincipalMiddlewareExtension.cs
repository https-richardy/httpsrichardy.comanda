namespace Comanda.Orchestrator.WebApi.Middlewares;

[ExcludeFromCodeCoverage(Justification = "contains only dependency injection")]
public static class PrincipalMiddlewareExtensions
{
    public static IApplicationBuilder UsePrincipalMiddleware(this IApplicationBuilder app)
    {
        return app.UseMiddleware<PrincipalMiddleware>();
    }
}
