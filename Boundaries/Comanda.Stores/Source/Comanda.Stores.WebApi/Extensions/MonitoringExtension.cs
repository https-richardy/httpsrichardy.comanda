namespace Comanda.Stores.WebApi.Extensions;

[ExcludeFromCodeCoverage(Justification = "contains only http pipeline configuration with no business logic.")]
public static class MonitoringExtension
{
    public static void AddMonitoring(this WebApplicationBuilder builder)
    {
        // prevents sentry from being initialized in non-production/non-development environments (e.g., testing)
        if (!builder.Environment.IsDevelopment() && !builder.Environment.IsProduction())
            return;

        var settings = builder.Services
            .BuildServiceProvider()
            .GetRequiredService<ISettings>();

        // https://docs.sentry.io/platforms/dotnet/
        builder.WebHost.UseSentry(options =>
        {
            options.Dsn = settings.Observability.SentryDsn;
            options.Environment = builder.Environment.EnvironmentName;
            options.TracesSampleRate = 1.0;
            options.Debug = true;
            options.EnableLogs = true;
        });
    }
}
