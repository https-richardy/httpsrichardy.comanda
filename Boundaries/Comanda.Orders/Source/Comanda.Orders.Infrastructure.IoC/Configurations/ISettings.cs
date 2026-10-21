namespace Comanda.Orders.CrossCutting.Configurations;

public interface ISettings
{
    public DatabaseSettings Database { get; }
    public FederationSettings Federation { get; }
    public ObservabilitySettings Observability { get; }
}
