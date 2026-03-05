namespace Comanda.Subscriptions.WebApi.Middlewares;

[ExcludeFromCodeCoverage(Justification = "contains only dependency injection")]
public static class CorrelationMiddlewareExtension
{
    public static IApplicationBuilder UseCorrelationMiddleware(this IApplicationBuilder app)
    {
        return app.UseMiddleware<CorrelationMiddleware>();
    }
}
