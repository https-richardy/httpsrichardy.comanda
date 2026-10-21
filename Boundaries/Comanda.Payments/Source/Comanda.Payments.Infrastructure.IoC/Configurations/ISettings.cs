namespace Comanda.Payments.Infrastructure.IoC.Configurations;

public interface ISettings
{
    public DatabaseSettings Database { get; }
    public AuthenticationSettings Authentication { get; }
    public AbacatePaySettings AbacatePay { get; }
    public ObservabilitySettings Observability { get; }
}
