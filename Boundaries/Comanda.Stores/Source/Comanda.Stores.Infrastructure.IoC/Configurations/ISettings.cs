namespace Comanda.Stores.Infrastructure.IoC.Configurations;

public interface ISettings
{
    public DatabaseSettings Database { get; }
    public FederationSettings Federation { get; }
    public ObservabilitySettings Observability { get; }
}
