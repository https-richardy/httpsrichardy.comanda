namespace Comanda.Subscriptions.CrossCutting.Configurations;

public sealed record Settings : ISettings
{
    public DatabaseSettings Database { get; init; } = default!;
    public FederationSettings Federation { get; init; } = default!;
    public StripeSettings Stripe { get; init; } = default!;
    public ObservabilitySettings Observability { get; init; } = default!;
}