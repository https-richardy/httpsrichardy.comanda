namespace Comanda.Payments.CrossCutting.Configurations;

public interface ISettings
{
    public DatabaseSettings Database { get; }
    public FederationSettings Federation { get; }
    public AbacatePaySettings AbacatePay { get; }
    public ObservabilitySettings Observability { get; }
}
