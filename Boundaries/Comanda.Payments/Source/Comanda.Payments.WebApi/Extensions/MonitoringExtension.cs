namespace Comanda.Payments.WebApi.Extensions;

public static class MonitoringExtension
{
    public static void AddMonitoring(this WebApplicationBuilder builder)
    {
        var settings = builder.Services
            .BuildServiceProvider()
            .GetRequiredService<ISettings>();

        // https://docs.sentry.io/platforms/dotnet/
        builder.WebHost.UseSentry(options =>
        {
            options.Dsn = settings.Observability.SentryDsn;
            options.Environment = builder.Environment.EnvironmentName;
            options.TracesSampleRate = 1.0;
            options.EnableLogs = true;
        });
    }
}
