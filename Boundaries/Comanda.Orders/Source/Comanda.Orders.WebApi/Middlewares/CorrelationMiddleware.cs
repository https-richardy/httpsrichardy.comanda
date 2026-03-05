namespace Comanda.Orders.WebApi.Middlewares;

public sealed class CorrelationMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var correlationId = context.Request.Headers[Headers.Correlation].FirstOrDefault();

        if (string.IsNullOrWhiteSpace(correlationId))
            correlationId = context.TraceIdentifier;

        context.Items[Headers.Correlation] = correlationId;
        context.Response.Headers[Headers.Correlation] = correlationId;

        /* enriches the logging scope with the current correlation identifier, ensuring all logs within this request pipeline share the same identifier */
        /* more details: https://microsoft.github.io/code-with-engineering-playbook/observability/correlation-id/ */

        using (LogContext.PushProperty(Headers.Correlation, correlationId))
        using (SentrySdk.PushScope())
        {
            SentrySdk.ConfigureScope(scope =>
            {
                scope.SetTag("correlation_id", correlationId);
            });

            await next(context);
        }
    }
}
