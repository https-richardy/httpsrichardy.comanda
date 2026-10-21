namespace Comanda.Subscriptions.Infrastructure.IoC.Configurations;

public interface ISettings
{
    public DatabaseSettings Database { get; }
    public AuthenticationSettings Authentication { get; }
    public StripeSettings Stripe { get; }
    public ObservabilitySettings Observability { get; }
}