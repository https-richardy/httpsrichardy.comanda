namespace Comanda.Subscriptions.CrossCutting.Configurations;

public interface ISettings
{
    public DatabaseSettings Database { get; }
    public FederationSettings Federation { get; }
    public StripeSettings Stripe { get; }
    public ObservabilitySettings Observability { get; }
}