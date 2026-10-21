namespace Comanda.Subscriptions.Infrastructure.IoC.Configurations;

public sealed record ObservabilitySettings
{
    public string SeqServerUrl { get; init; } = default!;
    public string SentryDsn { get; init; } = default!;
}
