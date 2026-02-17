namespace Comanda.Payments.WebApi.Extensions;

[ExcludeFromCodeCoverage(Justification = "contains only http pipeline configuration with no business logic.")]
public static class ObservabilityExtension
{
    public static void AddObservability(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((context, services, logger) =>
        {
            var settings = services.GetRequiredService<ISettings>();

            logger
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.Seq(settings.Observability.SeqServerUrl)
                .WriteTo.Sentry(options =>
                {
                    options.Dsn = settings.Observability.SentryDsn;
                    options.TracesSampleRate = 1.0;
                    options.AttachStacktrace = true;
                });
        });
    }
}
