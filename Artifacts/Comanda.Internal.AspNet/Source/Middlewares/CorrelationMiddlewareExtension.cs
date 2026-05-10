namespace Comanda.Internal.AspNet.Middlewares;

public static class CorrelationMiddlewareExtension
{
    public static IApplicationBuilder UseCorrelationMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<CorrelationMiddleware>();
    }
}
