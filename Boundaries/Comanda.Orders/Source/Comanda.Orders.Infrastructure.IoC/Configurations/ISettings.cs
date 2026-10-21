namespace Comanda.Orders.Infrastructure.IoC.Configurations;

public interface ISettings
{
    public DatabaseSettings Database { get; }
    public AuthenticationSettings Authentication { get; }
    public ObservabilitySettings Observability { get; }
}
